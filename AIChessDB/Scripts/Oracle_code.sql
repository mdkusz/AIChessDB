create or replace PACKAGE STATISTICS AS 

-- Keyword types
KT_PositionSts CONSTANT VARCHAR2(3) := 'PST';
KT_MatchSts CONSTANT VARCHAR2(3) := 'MST';
KT_MatchKw CONSTANT VARCHAR2(3) := 'MKW';

-- Move Events
EV_CHECK CONSTANT NUMBER(16) := 1;
EV_CHECKMATE CONSTANT NUMBER(16) := 2;
EV_MULTIPLECHECK CONSTANT NUMBER(16) := 4;
EV_DISCOVEREDCHECK CONSTANT NUMBER(16) := 8;
EV_DRAWOFFER CONSTANT NUMBER(16) := 16;
EV_PAWNPROMOTED CONSTANT NUMBER(16) := 32;
EV_ENPASSANT CONSTANT NUMBER(16) := 64;
EV_KINGSIDECASTLING CONSTANT NUMBER(16) := 256;
EV_QUEENSIDECASTLING CONSTANT NUMBER(16) := 512;
EV_MOVE CONSTANT NUMBER(16) := 2097152;
-- Moving piece or pawn promotion
EV_PAWN1 CONSTANT NUMBER(16) := 1024;
EV_WHITEBISHOP1 CONSTANT NUMBER(16) := 2048;
EV_BLACKBISHOP1 CONSTANT NUMBER(16) := 4194304;
EV_KNIGHT1 CONSTANT NUMBER(16) := 4096;
EV_ROOK1 CONSTANT NUMBER(16) := 8192;
EV_QUEEN1 CONSTANT NUMBER(16) := 16384;
EV_KING1 CONSTANT NUMBER(16) := 32768;

EV_CAPTURE CONSTANT NUMBER(16) := 128;
-- Captured piece
EV_PAWN2 CONSTANT NUMBER(16) := 65536;
EV_WHITEBISHOP2 CONSTANT NUMBER(16) := 131072;
EV_BLACKBISHOP2 CONSTANT NUMBER(16) := 8388608;
EV_KNIGHT2 CONSTANT NUMBER(16) := 262144;
EV_ROOK2 CONSTANT NUMBER(16) := 524288;
EV_QUEEN2 CONSTANT NUMBER(16) := 1048576;

-- Move player
WHITETOMOVE CONSTANT NUMBER(1) := 0;
BLACKTOMOVE CONSTANT NUMBER(1) := 1;

PROCEDURE GetOrCreateKeyword(Keyw keywords.keyword%type,
    KwType keywords.keyword_type%TYPE,
    DescKwd keywords.description%type,
    KwCode out keywords.cod_keyword%type);

PROCEDURE CreateKeywordValue(KValue keyword_values.kw_value%type,
    VCode out keyword_values.cod_value%type);

PROCEDURE UpdatePositionStatistic(Position positions.cod_position%type,
    Keyw keywords.keyword%type,
    KwType keywords.keyword_type%TYPE,
    DescKwd keywords.description%type,
    StValue position_statistics.st_value%type);
    
PROCEDURE UpdateMatchStatistic(Match matches.cod_match%type,
    Keyw keywords.keyword%type,
    KwType keywords.keyword_type%TYPE,
    DescKwd keywords.description%type,
    StValue position_statistics.st_value%type);

PROCEDURE UpdateStatistics;

END STATISTICS;

create or replace PACKAGE BODY STATISTICS AS

PROCEDURE GetOrCreateKeyword(Keyw keywords.keyword%type,
    KwType keywords.keyword_type%TYPE,
    DescKwd keywords.description%type,
    KwCode out keywords.cod_keyword%type)
IS
BEGIN
    select cod_keyword into KwCode
        from keywords
        where keyword_type = KwType
            and keyword = Keyw;
EXCEPTION WHEN no_data_found THEN        
    insert into keywords(cod_keyword,keyword,description,keyword_type)
        values(seq_keywords.nextval,Keyw,DescKwd,KwType)
        returning cod_keyword into KwCode;
END GetOrCreateKeyword;

PROCEDURE CreateKeywordValue(KValue keyword_values.kw_value%type,
    VCode out keyword_values.cod_value%type)
IS
BEGIN
    INSERT INTO keyword_values(kw_value)
        VALUES(KValue)
        RETURNING cod_value INTO VCode;
END CreateKeywordValue;

PROCEDURE UpdatePositionStatistic(Position positions.cod_position%type,
    Keyw keywords.keyword%type,
    KwType keywords.keyword_type%TYPE,
    DescKwd keywords.description%type,
    StValue position_statistics.st_value%type) IS
    KwCode number;
    Cnt number;
BEGIN
    GetOrCreateKeyword(Keyw, KwType, DescKwd, KwCode);
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
END UpdatePositionStatistic;

PROCEDURE UpdateMatchStatistic(Match matches.cod_match%type,
    Keyw keywords.keyword%type,
    KwType keywords.keyword_type%TYPE,
    DescKwd keywords.description%type,
    StValue position_statistics.st_value%type) IS
    KwCode number;
    Cnt number;
BEGIN
    GetOrCreateKeyword(Keyw, KwType, DescKwd, KwCode);
    select count(*) into Cnt 
        from match_statistics
        where cod_keyword = KwCode
            and cod_match = Match;
    if Cnt > 0 then
        update match_statistics set st_value = StValue 
            where cod_keyword = KwCode
                and cod_match = Match;
    else 
        insert into match_statistics(cod_match, cod_keyword, st_value)
            values(Match, KwCode, StValue);
    end if;
END UpdateMatchStatistic;

PROCEDURE UpdateStatistics
IS
    CURSOR cpos is SELECT COUNT(distinct mp.cod_match) cntm,count(*) cntt,pm.cod_position
        FROM MATCH_POSITIONS mp, positions pm
        WHERE mp.cod_position = pm.cod_position 
        AND pm.sts_date is null
        group by pm.cod_position;
BEGIN
    for pos in cpos loop
        UpdatePositionStatistic(pos.cod_position,
            'PST_MATCHCOUNT',
            'PST',
            'Matches where this position has occurred',
            pos.cntm);
        UpdatePositionStatistic(pos.cod_position,
            'PST_OCCURCOUNT',
            'PST',
            'Times this position has occurred',
            pos.cntt); 
		update positions set sts_date = SYSDATE where COD_POSITION = pos.cod_position;
		commit;
    end loop;
	
END UpdateStatistics;

END STATISTICS;

create or replace PROCEDURE CreateKeywordValue(KValue keyword_values.kw_value%type,
    VCode out keyword_values.cod_value%type)
IS
BEGIN
    INSERT INTO keyword_values(kw_value)
        VALUES(KValue)
        RETURNING cod_value INTO VCode;
END CreateKeywordValue;

declare
job number;
begin
    dbms_job.submit(job,'STATISTICS.UpdateStatistics;',sysdate,'SYSDATE+(5/1440)');
    commit;
end;
