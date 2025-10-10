use chess_pgn;
delimiter $$
CREATE PROCEDURE GetOrCreateKeyword(Keyw VARCHAR(250),
    KwType VARCHAR(3),
    DescKwd VARCHAR(4000),
    out KwCode bigint unsigned)
BEGIN
    select cod_keyword into KwCode
        from keywords
        where keyword_type = KwType
            and keyword = Keyw;
	IF KwCode is null THEN        
		insert into keywords(keyword,description,keyword_type)
			values(Keyw,DescKwd,KwType);
		select last_insert_id() into KwCode;
	END IF;
END$$

CREATE PROCEDURE CreateKeywordValue(KValue VARCHAR(500),
    out VCode bigint unsigned)
BEGIN
    insert into keyword_values(kw_value)
	    values(KValue);
	select last_insert_id() into VCode;
END$$

CREATE PROCEDURE UpdatePositionStatistic(Position bigint unsigned,
    Keyw varchar(250),
    KwType varchar(3),
    DescKwd varchar(4000),
    StValue double)
BEGIN
	declare KwCode bigint unsigned;
    declare Cnt int;

    call GetOrCreateKeyword(Keyw, KwType, DescKwd, KwCode);
    select count(*) into Cnt 
        from position_statistics
        where cod_keyword = KwCode
            and cod_position = Position;
    if Cnt > 0 then
        update position_statistics set st_value = StValue 
            where cod_keyword = KwCode
                and cod_position = Position;
    else 
        insert into position_statistics(cod_position, cod_keyword, st_value)
            values(Position, KwCode, StValue);
    end if;
END$$

CREATE PROCEDURE UpdateMatchStatistic(cMatch bigint unsigned,
    Keyw varchar(250),
    KwType varchar(3),
    DescKwd varchar(4000),
    StValue double)
BEGIN
    declare KwCode bigint unsigned;
    declare Cnt int;

    call GetOrCreateKeyword(Keyw, KwType, DescKwd, KwCode);
    select count(*) into Cnt 
        from match_statistics
        where cod_keyword = KwCode
            and cod_match = cMatch;
    if Cnt > 0 then
        update match_statistics set st_value = StValue 
            where cod_keyword = KwCode
                and cod_match = cMatch;
    else 
        insert into match_statistics(cod_match, cod_keyword, st_value)
            values(cMatch, KwCode, StValue);
    end if;
END$$

CREATE PROCEDURE UpdateStatistics()
BEGIN
	DECLARE codp bigint unsigned;
	declare cntm double;
	declare cntt double;
	DECLARE finished INTEGER DEFAULT 0;

    declare cpos CURSOR for SELECT COUNT(distinct mp.cod_match) cntm,count(*) cntt,pm.cod_position
        FROM MATCH_POSITIONS mp, positions pm
        WHERE mp.cod_position = pm.cod_position 
        AND pm.sts_date is null
        group by pm.cod_position;
	DECLARE CONTINUE HANDLER 
        FOR NOT FOUND SET finished = 1;
    
    OPEN cpos;
	
    lUpdate: loop
		FETCH cpos INTO cntm,cntt,codp;
        IF finished = 1 THEN
			LEAVE lUpdate;
		END IF;            
        call UpdatePositionStatistic(codp,
            'PST_MATCHCOUNT',
            'PST',
            'Matches where this position has occurred',
            cntm);
        call UpdatePositionStatistic(codp,
            'PST_OCCURCOUNT',
            'PST',
            'Times this position has occurred',
            cntt); 
		update positions set sts_date = SYSDATE() where COD_POSITION = codp;
    end loop lUpdate;
	
    CLOSE cpos;
END$$

delimiter ;

CREATE EVENT UpdateSts
ON SCHEDULE EVERY 10 MINUTE
DO
call UpdateStatistics();

