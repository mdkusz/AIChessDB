USE [CHESSPOSDB]
GO
/****** Object:  StoredProcedure [dbo].[GetOrCreateKeyword]    Script Date: 06/04/2020 12:46:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Miguel Díaz Kusztrich
-- Create date: 30/03/2020
-- Description:	Create a new Keyword if not already created, returns the code of the keyword
-- =============================================
CREATE PROCEDURE [dbo].[GetOrCreateKeyword]
(
	@Keyw NVARCHAR(250), 
	@KwType VARCHAR(3),
	@DescKwd NVARCHAR(4000),
	@KwCode BigInt output
)	
AS
BEGIN
	SET NOCOUNT ON;

	select @KwCode = cod_keyword
        from keywords WITH (NOLOCK)
        where keyword_type = @KwType
            and keyword = @Keyw
	IF @KwCode is null
	BEGIN
		insert into keywords(keyword,description,keyword_type)
			values(@Keyw,@DescKwd,@KwType)
		SELECT @KwCode = SCOPE_IDENTITY()
	END
END
GO
-- =============================================
-- Author:		Miguel Díaz Kusztrich
-- Create date: 14/07/2025
-- Description:	Create a new Keyword value
-- =============================================
CREATE PROCEDURE [dbo].[CreateKeywordValue]
(
	@KValue NVARCHAR(500), 
	@VCode BigInt output
)	
AS
BEGIN
	SET NOCOUNT ON;

	insert into keyword_values(kw_value)
		values(@KValue)
	SELECT @VCode = SCOPE_IDENTITY()
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateMatchStatistic]    Script Date: 06/04/2020 12:50:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Miguel Díaz Kusztrich
-- Create date: 30/03/2020
-- Description:	Update statistic value for a given match
-- =============================================
CREATE PROCEDURE [dbo].[UpdateMatchStatistic]
(
	@Match BigInt,
	@Keyw NVARCHAR(250), 
	@KwType VARCHAR(3),
	@DescKwd NVARCHAR(4000),
	@StValue decimal(20,4)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @KwCode BigInt
	DECLARE @Cnt Int

	exec GetOrCreateKeyword @Keyw, @KwType, @DescKwd, @KwCode OUTPUT
    select @Cnt = count(*)
        from match_statistics WITH (NOLOCK)
        where cod_keyword = @KwCode
            and cod_match = @Match;
    if @Cnt > 0 
        update match_statistics set st_value = @StValue 
            where cod_keyword = @KwCode
                and cod_match = @Match
    else 
        insert into match_statistics(cod_match, cod_keyword, st_value)
            values(@Match, @KwCode, @StValue)
END
GO
/****** Object:  StoredProcedure [dbo].[UpdatePositionStatistic]    Script Date: 06/04/2020 12:51:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Miguel Díaz Kusztrich
-- Create date: 30/03/2020
-- Description:	Update statistics value for a given position
-- =============================================
CREATE PROCEDURE [dbo].[UpdatePositionStatistic]
(
	@Position BigInt,
	@Keyw NVARCHAR(250), 
	@KwType VARCHAR(3),
	@DescKwd NVARCHAR(4000),
	@StValue decimal(20,4)
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @KwCode BigInt
	DECLARE @Cnt Int

	exec GetOrCreateKeyword @Keyw, @KwType, @DescKwd, @KwCode OUTPUT

	select @Cnt = count(*) 
        from position_statistics WITH (NOLOCK)
        where cod_keyword = @KwCode
            and cod_position = @Position

	IF @Cnt > 0 
		update position_statistics set st_value = @StValue 
            where cod_keyword = @KwCode
                and cod_position = @Position		
	else
		insert into position_statistics(cod_position, cod_keyword, st_value)
            values(@Position, @KwCode, @StValue)
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateStatistics]    Script Date: 06/04/2020 12:52:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Miguel Díaz Kusztrich
-- Create date: 30/03/2020
-- Description:	Calculate match statistics
-- =============================================
ALTER PROCEDURE [dbo].[UpdateStatistics]
AS
BEGIN
	SET NOCOUNT ON;

	CREATE TABLE #TmpTable (cntm int, cntt int, Position BigInt)
	INSERT INTO #TmpTable(cntm, cntt, Position) (SELECT COUNT(distinct mp.cod_match) cntm,count(*) cntt,pm.cod_position
        FROM MATCH_POSITIONS mp WITH (NOLOCK), positions pm WITH (NOLOCK)
        WHERE mp.cod_position = pm.cod_position 
			AND pm.sts_date is null
        group by pm.cod_position)
				
	DECLARE CURSOR_MP CURSOR FOR
		SELECT cntm,cntt,Position FROM #TmpTable
	BEGIN TRY
		DECLARE @Cntt int
		DECLARE @Cntm int
		DECLARE @Position Bigint
		OPEN CURSOR_MP
		FETCH next from CURSOR_MP INTO @Cntm, @Cntt, @Position
		WHILE @@fetch_status = 0
		BEGIN	
			BEGIN TRANSACTION
			exec UpdatePositionStatistic @Position,
				'PST_MATCHCOUNT',
				'PST',
				'Matches where this position has occurred',
				@Cntm
			exec UpdatePositionStatistic @Position,
				'PST_OCCURCOUNT',
				'PST',
				'Times this position has occurred',
				@Cntt
			update positions set sts_date = GETDATE() where COD_POSITION = @Position
			COMMIT
			FETCH next from CURSOR_MP INTO @Cntm, @Cntt, @Position
		END
		CLOSE CURSOR_MP
		DEALLOCATE CURSOR_MP
		DROP TABLE #TmpTable
	END TRY
	BEGIN CATCH
		ROLLBACK
		CLOSE CURSOR_MP
		DEALLOCATE CURSOR_MP
		DROP TABLE #TmpTable	
		declare @ErrorMessage nvarchar(max), @ErrorSeverity int, @ErrorState int;
		select @ErrorMessage = ERROR_MESSAGE() + ' Line ' + cast(ERROR_LINE() as nvarchar(5)), 
			@ErrorSeverity = ERROR_SEVERITY(), 
			@ErrorState = ERROR_STATE();
		raiserror (@ErrorMessage, @ErrorSeverity, @ErrorState)
	END CATCH
END
GO
USE [msdb]
GO

/****** Object:  Job [UpdateMatchesJob]    Script Date: 08/04/2020 11:20:25 ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 08/04/2020 11:20:25 ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'UpdateMatchesJob', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'Update match statistics', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Call procedure]    Script Date: 08/04/2020 11:20:25 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Call procedure', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'begin
exec UpdateStatistics
end', 
		@database_name=N'CHESSPOSDB', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Cada 10 minutos', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=4, 
		@freq_subday_interval=10, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20200330, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959, 
		@schedule_uid=N'849f1455-eb1c-4be8-ad6a-26b1cecc3c63'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:
GO