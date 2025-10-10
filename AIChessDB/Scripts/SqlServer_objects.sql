USE [CHESSPOSDB]
GO

/****** Object:  Table [dbo].[Autores]    Script Date: 25/03/2020 16:30:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
/* uncomment to drop all objects and recreate them
drop table move_comments
GO

drop table match_keywords
GO

drop table match_statistics
GO

drop table position_statistics
GO

drop table keyword_values
GO

drop table keywords
GO

drop table match_moves
GO

drop table match_positions
GO

drop table matches
GO

drop table positions
GO

drop table match_events
GO

drop table token_usage
GO

*/
CREATE TABLE [dbo].[match_events](
	[COD_EVENT] [bigint] NOT NULL,
	[DESCRIPTION] [varchar](50) NOT NULL,
 CONSTRAINT [PK_EVENTS] PRIMARY KEY CLUSTERED 
(
	[COD_EVENT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

insert into match_events(cod_event,description)
values(1,'EVENT_CHECK')
GO

insert into match_events(cod_event,description)
values(2,'EVENT_CHECK_MATE')
GO

insert into match_events(cod_event,description)
values(4,'EVENT_MULTIPLE_CHECK')
GO

insert into match_events(cod_event,description)
values(8,'EVENT_DISCOVERED_CHECK')
GO

insert into match_events(cod_event,description)
values(16,'EVENT_DRAW_OFFER')
GO

insert into match_events(cod_event,description)
values(32,'EVENT_PAWN_PROMOTED')
GO

insert into match_events(cod_event,description)
values(64,'EVENT_PAWN_PASSANT')
GO

insert into match_events(cod_event,description)
values(128,'EVENT_CAPTURE')
GO

insert into match_events(cod_event,description)
values(256,'EVENT_CASTLEK')
GO

insert into match_events(cod_event,description)
values(512,'EVENT_CASTLEQ')
GO

insert into match_events(cod_event,description)
values(1024,'EVENT_PAWN1')
GO

insert into match_events(cod_event,description)
values(2048,'EVENT_WBISHOP1')
GO

insert into match_events(cod_event,description)
values(4096,'EVENT_KNIGHT1')
GO

insert into match_events(cod_event,description)
values(8192,'EVENT_ROOK1')
GO

insert into match_events(cod_event,description)
values(16384,'EVENT_QUEEN1')
GO

insert into match_events(cod_event,description)
values(32768,'EVENT_KING1')
GO

insert into match_events(cod_event,description)
values(65536,'EVENT_PAWN2')
GO

insert into match_events(cod_event,description)
values(131072,'EVENT_WBISHOP2')
GO

insert into match_events(cod_event,description)
values(262144,'EVENT_KNIGHT2')
GO

insert into match_events(cod_event,description)
values(524288,'EVENT_ROOK2')
GO

insert into match_events(cod_event,description)
values(1048576,'EVENT_QUEEN2')
GO

insert into match_events(cod_event,description)
values(2097152,'EVENT_MOVE')
GO

insert into match_events(cod_event,description)
values(4194304,'EVENT_BBISHOP1')
GO

insert into match_events(cod_event,description)
values(8388608,'EVENT_BBISHOP2')
GO

insert into match_events(cod_event,description)
values(16777216,'EVENT_BISHOP3')
GO

insert into match_events(cod_event,description)
values(33554432,'EVENT_KNIGHT3')
GO

insert into match_events(cod_event,description)
values(67108864,'EVENT_ROOK3')
GO

insert into match_events(cod_event,description)
values(134217728,'EVENT_QUEEN3')
GO

CREATE TABLE [dbo].[keywords](
	[COD_KEYWORD] [bigint] IDENTITY(1,1) NOT NULL,
	[KEYWORD] [nvarchar](250) NOT NULL,
	[KEYWORD_TYPE] [varchar](3) DEFAULT 'MKW',
	[DESCRIPTION] [nvarchar](4000),
 CONSTRAINT [PK_KEYWORDS] PRIMARY KEY CLUSTERED 
(
	[COD_KEYWORD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
CONSTRAINT UK_KEYWORDS UNIQUE NONCLUSTERED(
	[KEYWORD],[KEYWORD_TYPE]
)WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[keyword_values](
	[COD_VALUE] [bigint] IDENTITY(1,1) NOT NULL,
	[KW_VALUE] [nvarchar](500),
 CONSTRAINT [PK_KEY_VALUES] PRIMARY KEY CLUSTERED 
(
	[COD_VALUE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[positions](
	[COD_POSITION] [bigint] IDENTITY(1,1) NOT NULL,
	[BOARD] [varchar](64) NOT NULL,
	black_pawns tinyint default 0,
	white_pawns tinyint default 0,
	black_rooks tinyint default 0,
	white_rooks tinyint default 0,
	black_bishops tinyint default 0,
	white_bishops tinyint default 0,
	black_knights tinyint default 0,
	white_knights tinyint default 0,
	black_queens tinyint default 0,
	white_queens tinyint default 0,
	sts_date datetime,
 CONSTRAINT [PK_POSITIONS] PRIMARY KEY CLUSTERED 
(
	[COD_POSITION] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
CONSTRAINT UK_BOARD UNIQUE NONCLUSTERED(
	[BOARD]
)WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[position_statistics](
	[COD_KEYWORD] [bigint] NOT NULL,
	[COD_POSITION] [bigint] NOT NULL,
	[ST_VALUE] [decimal](20,4),
 CONSTRAINT [PK_POSSTS] PRIMARY KEY CLUSTERED 
(
	[COD_KEYWORD] ASC, [COD_POSITION] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[position_statistics]  WITH CHECK ADD  CONSTRAINT [FK_PS_POSITION] FOREIGN KEY([COD_POSITION])
REFERENCES [dbo].[positions] ([COD_POSITION])
GO

ALTER TABLE [dbo].[position_statistics] CHECK CONSTRAINT [FK_PS_POSITION]
GO

ALTER TABLE [dbo].[position_statistics]  WITH CHECK ADD  CONSTRAINT [FK_PS_KEYWORD] FOREIGN KEY([COD_KEYWORD])
REFERENCES [dbo].[keywords] ([COD_KEYWORD])
GO

ALTER TABLE [dbo].[position_statistics] CHECK CONSTRAINT [FK_PS_KEYWORD]
GO

CREATE TABLE [dbo].[matches](
	[COD_MATCH] [bigint] IDENTITY(1,1) NOT NULL,
	[INITIAL_POSITION] [bigint] NOT NULL,
	[MATCH_DESCRIPTION] [NVARCHAR](1000),
	[MATCH_DATE] [VARCHAR](30),
	[WHITE] [NVARCHAR](100),
	[BLACK] [NVARCHAR](100),
    [RESULT] [tinyint] NOT NULL,
	[RESULT_TEXT] [VARCHAR](8) NOT NULL,
	[MOVE_COUNT] [smallint] NOT NULL,
	[FULLMOVE_COUNT] [smallint] NOT NULL,
	sts_date datetime,
	creation_date datetime default getdate(),
	sha256 VARBINARY(32),
 CONSTRAINT [PK_MATCHES] PRIMARY KEY CLUSTERED 
(
	[COD_MATCH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[matches]  WITH CHECK ADD  CONSTRAINT [FK_INITIAL_POSITION] FOREIGN KEY([INITIAL_POSITION])
REFERENCES [dbo].[positions] ([COD_POSITION])
GO

ALTER TABLE [dbo].[matches] CHECK CONSTRAINT [FK_INITIAL_POSITION]
GO

CREATE TABLE [dbo].[match_statistics](
	[COD_KEYWORD] [bigint] NOT NULL,
	[COD_MATCH] [bigint] NOT NULL,
	[ST_VALUE] [decimal](20,4),
 CONSTRAINT [PK_MATCHSTS] PRIMARY KEY CLUSTERED 
(
	[COD_KEYWORD] ASC, [COD_MATCH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[match_statistics]  WITH CHECK ADD  CONSTRAINT [FK_MS_MATCH] FOREIGN KEY([COD_MATCH])
REFERENCES [dbo].[matches] ([COD_MATCH])
GO

ALTER TABLE [dbo].[match_statistics] CHECK CONSTRAINT [FK_MS_MATCH]
GO

ALTER TABLE [dbo].[match_statistics]  WITH CHECK ADD  CONSTRAINT [FK_MS_KEYWORD] FOREIGN KEY([COD_KEYWORD])
REFERENCES [dbo].[keywords] ([COD_KEYWORD])
GO

ALTER TABLE [dbo].[match_statistics] CHECK CONSTRAINT [FK_MS_KEYWORD]
GO

CREATE TABLE [dbo].[match_positions](
	[COD_MATCH] [bigint] NOT NULL,
	[COD_POSITION] [bigint] NOT NULL,
	[POSITION_ORDER] [smallint] NOT NULL,
	[POSITION_EVENTS] [bigint] DEFAULT 0,
	[SCORE] [DECIMAL](11,10),
	sts_date datetime,
 CONSTRAINT [PK_MATCH_POSITIONS] PRIMARY KEY CLUSTERED 
(
	[COD_MATCH] ASC, [COD_POSITION] ASC, [POSITION_ORDER] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[match_positions]  WITH CHECK ADD  CONSTRAINT [FK_MP_POSITION] FOREIGN KEY([COD_POSITION])
REFERENCES [dbo].[positions] ([COD_POSITION])
GO

ALTER TABLE [dbo].[match_positions] CHECK CONSTRAINT [FK_MP_POSITION]
GO

ALTER TABLE [dbo].[match_positions]  WITH CHECK ADD  CONSTRAINT [FK_MP_MATCH] FOREIGN KEY([COD_MATCH])
REFERENCES [dbo].[matches] ([COD_MATCH])
GO

ALTER TABLE [dbo].[match_positions] CHECK CONSTRAINT [FK_MP_MATCH]
GO

CREATE TABLE [dbo].[match_moves](
	[COD_MATCH] [bigint] NOT NULL,
	[FROM_POSITION] [bigint] NOT NULL,
	[TO_POSITION] [bigint] NOT NULL,
	[MOVE_ORDER] [smallint] NOT NULL,
	[MOVE_NUMBER] [smallint] NOT NULL,
	[MOVE_PLAYER] [tinyint] NOT NULL,
	[MOVE_EVENTS] [bigint] default 0,
	[MOVE_FROM] [tinyint] NOT NULL,
	[MOVE_TO] [tinyint] NOT NULL,
	[MOVE_AN_TEXT] [VARCHAR](10),
	[SCORE] [DECIMAL](11,10),
	[COMMENTS] [TinyInt] default 0,
 CONSTRAINT [PK_MATCH_MOVES] PRIMARY KEY CLUSTERED 
(
	[COD_MATCH] ASC, [MOVE_ORDER] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
CONSTRAINT UK_MOVEPLAYER UNIQUE NONCLUSTERED(
	[COD_MATCH],[MOVE_NUMBER],[MOVE_PLAYER]
)WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[match_moves]  WITH CHECK ADD  CONSTRAINT [FK_FROM_POSITION] FOREIGN KEY([FROM_POSITION])
REFERENCES [dbo].[positions] ([COD_POSITION])
GO

ALTER TABLE [dbo].[match_moves] CHECK CONSTRAINT [FK_FROM_POSITION]
GO

ALTER TABLE [dbo].[match_moves]  WITH CHECK ADD  CONSTRAINT [FK_MMT_POSITION] FOREIGN KEY([COD_MATCH],[TO_POSITION],[MOVE_ORDER])
REFERENCES [dbo].[match_positions] ([COD_MATCH],[COD_POSITION],[POSITION_ORDER])
GO

ALTER TABLE [dbo].[match_moves] CHECK CONSTRAINT [FK_MMT_POSITION]
GO

ALTER TABLE [dbo].[match_moves]  WITH CHECK ADD  CONSTRAINT [FK_MM_MATCH] FOREIGN KEY([COD_MATCH])
REFERENCES [dbo].[matches] ([COD_MATCH])
GO

ALTER TABLE [dbo].[match_moves] CHECK CONSTRAINT [FK_MM_MATCH]
GO

CREATE TABLE [dbo].[match_keywords](
	[COD_KEYWORD] [bigint] NOT NULL,
	[COD_MATCH] [bigint] NOT NULL,
	[COD_VALUE] [bigint] NOT NULL,
 CONSTRAINT [PK_MKEYWORDS] PRIMARY KEY CLUSTERED 
(
	[COD_KEYWORD] ASC, [COD_MATCH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[match_keywords]  WITH CHECK ADD  CONSTRAINT [FK_MKV_MATCH] FOREIGN KEY([COD_MATCH])
REFERENCES [dbo].[matches] ([COD_MATCH])
GO

ALTER TABLE [dbo].[match_keywords] CHECK CONSTRAINT [FK_MKV_MATCH]
GO

ALTER TABLE [dbo].[match_keywords]  WITH CHECK ADD  CONSTRAINT [FK_MKV_KEYWORD] FOREIGN KEY([COD_KEYWORD])
REFERENCES [dbo].[keywords] ([COD_KEYWORD])
GO

ALTER TABLE [dbo].[match_keywords] CHECK CONSTRAINT [FK_MKV_KEYWORD]
GO

ALTER TABLE [dbo].[match_keywords]  WITH CHECK ADD  CONSTRAINT [FK_MKV_VALUE] FOREIGN KEY([COD_VALUE])
REFERENCES [dbo].[keyword_values] ([COD_VALUE])
GO

ALTER TABLE [dbo].[match_keywords] CHECK CONSTRAINT [FK_MKV_VALUE]
GO

CREATE TABLE [dbo].[move_comments](
	[COD_COMMENT] [bigint] IDENTITY(1,1) NOT NULL,
	[COD_MATCH] [bigint] NOT NULL,
	[MOVE_ORDER] [smallint] NOT NULL,
	[COMMENT_TEXT] [NVARCHAR](4000),
	[COMMENT_ORDER] [smallint] DEFAULT 0,
 CONSTRAINT [PK_MOVE_COMMENTS] PRIMARY KEY CLUSTERED 
(
	[COD_COMMENT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[move_comments]  WITH CHECK ADD  CONSTRAINT [FK_MOVE_COMMENT] FOREIGN KEY([COD_MATCH],[MOVE_ORDER])
REFERENCES [dbo].[match_moves] ([COD_MATCH],[MOVE_ORDER])
GO

ALTER TABLE [dbo].[move_comments] CHECK CONSTRAINT [FK_MOVE_COMMENT]
GO

CREATE NONCLUSTERED INDEX [IX_MATCHMOVECOUNT] ON [dbo].[matches]
(
	[MOVE_COUNT] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

create nonclustered index [IX_POSPZCOUNT] on [dbo].[positions]
(
white_queens asc,
black_queens asc,
white_rooks asc,
black_rooks asc,
white_bishops asc,
black_bishops asc,
white_knights asc,
black_knights asc,
white_pawns asc,
black_pawns asc
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX IX_POSITIONSTATS
ON [dbo].[positions] ([sts_date])
INCLUDE ([COD_POSITION])
GO

CREATE NONCLUSTERED INDEX [IX_KEYVALUE] ON [dbo].[keyword_values]
(
	[COD_VALUE] ASC,
	[KW_VALUE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

CREATE INDEX ix_matches_sha256 ON matches(sha256)
GO

create view VW_MATCH_POSITIONS AS
select m.COD_MATCH,m.INITIAL_POSITION,pi.board as initial_board,pi.black_pawns as initial_black_pawns,pi.white_pawns as initial_white_pawns,pi.black_rooks as initial_black_rooks,
pi.white_rooks as initial_white_rooks,pi.black_bishops as initial_black_bishops,pi.white_bishops as initial_white_bishops,pi.black_knights as initial_black_knights,
pi.white_knights as initial_white_knights,pi.black_queens as initial_black_queens,pi.white_queens as initial_white_queens,m.MATCH_DESCRIPTION,m.MATCH_DATE,m.WHITE,m.BLACK,
m.RESULT,m.RESULT_TEXT,m.MOVE_COUNT,m.FULLMOVE_COUNT,mp.POSITION_ORDER,mp.POSITION_EVENTS,mp.SCORE,mpn.board,mpn.black_pawns,mpn.white_pawns,mpn.black_rooks,
mpn.white_rooks,mpn.black_bishops,mpn.white_bishops,mpn.black_knights,mpn.white_knights,mpn.black_queens,mpn.white_queens
from matches m join positions pi on m.initial_position = pi.cod_position
join match_positions mp on mp.cod_match = m.cod_match join positions mpn on mp.cod_position = mpn.cod_position
GO

create view VW_MATCH_MOVES AS
select m.COD_MATCH,m.INITIAL_POSITION,pi.board as initial_board,pi.black_pawns as initial_black_pawns,pi.white_pawns as initial_white_pawns,pi.black_rooks as initial_black_rooks,
pi.white_rooks as initial_white_rooks,pi.black_bishops as initial_black_bishops,pi.white_bishops as initial_white_bishops,pi.black_knights as initial_black_knights,
pi.white_knights as initial_white_knights,pi.black_queens as initial_black_queens,pi.white_queens as initial_white_queens,m.MATCH_DESCRIPTION,m.MATCH_DATE,m.WHITE,m.BLACK,
m.RESULT,m.RESULT_TEXT,m.MOVE_COUNT,m.FULLMOVE_COUNT,mm.MOVE_ORDER,mm.MOVE_NUMBER,mm.MOVE_PLAYER,mm.MOVE_EVENTS,mm.MOVE_AN_TEXT,mm.COMMENTS,mm.SCORE,mm.MOVE_FROM,mm.FROM_POSITION,fp.board as from_board,fp.black_pawns as from_black_pawns,fp.white_pawns as from_white_pawns,fp.black_rooks as from_black_rooks,
fp.white_rooks as from_white_rooks,fp.black_bishops as from_black_bishops,fp.white_bishops as from_white_bishops,fp.black_knights as from_black_knights,fp.white_knights as from_white_knights,fp.black_queens as from_black_queens,fp.white_queens as from_white_queens,
mm.MOVE_TO,mm.TO_POSITION,tp.board as to_board,tp.black_pawns as to_black_pawns,tp.white_pawns as to_white_pawns,tp.black_rooks as to_black_rooks,
tp.white_rooks as to_white_rooks,tp.black_bishops as to_black_bishops,tp.white_bishops as to_white_bishops,tp.black_knights as to_black_knights,tp.white_knights as to_white_knights,tp.black_queens as to_black_queens,tp.white_queens as to_white_queens
from matches m join positions pi on m.initial_position = pi.cod_position
join match_moves mm on mm.cod_match = m.cod_match join positions fp on mm.from_position = fp.cod_position join positions tp on mm.to_position = tp.cod_position
GO

create view VW_MATCH_KEYWORDS AS
select m.COD_MATCH,m.MATCH_DESCRIPTION,m.MATCH_DATE,m.WHITE,m.BLACK,m.RESULT,m.RESULT_TEXT,m.MOVE_COUNT,m.FULLMOVE_COUNT,mk.COD_KEYWORD,k.KEYWORD,kv.kw_value,ks.KEYWORD statistic,ms.st_value 
from matches m join match_keywords mk on mk.COD_MATCH = m.COD_MATCH
join keywords k on k.COD_KEYWORD = mk.COD_KEYWORD
join keyword_values kv on kv.COD_VALUE = mk.COD_VALUE
join match_statistics ms on ms.COD_MATCH = m.COD_MATCH
join keywords ks on ks.COD_KEYWORD = ms.COD_KEYWORD
GO

CREATE TABLE [dbo].[token_usage](
	[COD_USAGE] [bigint] IDENTITY(1,1) NOT NULL,
    [USAGE_DATE] date not null,
	[PLAYER_NAME] nvarchar(50) not null,
	[MODEL_NAME] varchar(50) not null,
    [TEMPERATURE] DECIMAL(3,2),
	[TOP_P] DECIMAL(3,2),
	[INPUT_TOKENS] int default 0,
	[OUTPUT_TOKENS] int default 0,
	[AUDIO_INPUT_TOKENS] int default 0,
	[AUDIO_OUTPUT_TOKENS] int default 0,
	[REASONING_TOKENS] int default 0,
	[INPUT_CACHED_TOKENS] int default 0,
	[INPUT_IMAGE_TOKENS] int default 0,
	[INPUT_TEXT_TOKENS] int default 0
 CONSTRAINT [PK_TOKEN_USAGE] PRIMARY KEY CLUSTERED 
(
	[COD_USAGE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
