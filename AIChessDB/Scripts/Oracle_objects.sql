/* uncomment to drop all objects and recreate them
drop sequence seq_positions;

drop sequence seq_comments;

drop sequence seq_matches;

drop sequence seq_keywords;

drop sequence seq_kvalues;

drop table move_comments;

drop table match_keywords;

drop table match_statistics;

drop table position_statistics;

drop table keyword_values;

drop table keywords;

drop table match_moves;

drop table match_positions;

drop table matches;

drop table positions;

drop table match_events;

drop sequence seq_token_usage;

drop table token_usage

*/

/*
This is a chess game database. It contains the games and their moves, the positions of each move, the statistics of the positions and games, and the comments on the moves.
With this database, game analysis can be performed, such as determining the best move in a given position, or the best move in a given game.
You can also graphically represent the games and replay them move by move.
It allows searching for games by position, by player, by result, by date, by number of moves, by number of plays, by events, by comments, by statistics, etc.
It also allows searching for total or partial positions in all games.
*/

-- Those sequences are used to generate surrogate keys for the tables.
create sequence seq_positions nocache;

create sequence seq_comments nocache;

create sequence seq_matches nocache;

create sequence seq_keywords nocache;

create sequence seq_kvalues nocache;

create sequence seq_token_usage nocache;

/*
The match_events table contains the events that can occur in a chess game.
The events are represented by powers of 2, in the COD_EVENT field. In this way they can be combined into more complex events.
Usually, a simple event code is never found, but a combination of them, so if you are looking for a specific one, you have to use the BITAND function.
DESCRIPTION is the name of the event. It is not used in SQL, but it is used in the application to name translatable text resources.
*/

create table match_events(
	COD_EVENT NUMBER(16) NOT NULL,
	DESCRIPTION VARCHAR2(50) NOT NULL,
    constraint PK_EVENTS
        PRIMARY KEY(COD_EVENT)	
);

insert into match_events(cod_event,description)
values(1,'EVENT_CHECK');  -- This event represents a check

insert into match_events(cod_event,description)
values(2,'EVENT_CHECK_MATE'); -- This event represents a checkmate

insert into match_events(cod_event,description)
values(4,'EVENT_MULTIPLE_CHECK'); -- This event represents a multiple check

insert into match_events(cod_event,description)
values(8,'EVENT_DISCOVERED_CHECK'); -- This event represents a discovered check

insert into match_events(cod_event,description)
values(16,'EVENT_DRAW_OFFER');  -- This event represents an offer of a draw

insert into match_events(cod_event,description)
values(32,'EVENT_PAWN_PROMOTED');  -- This event represents a pawn promotion

insert into match_events(cod_event,description)
values(64,'EVENT_PAWN_PASSANT');   -- This event represents an en passant capture

insert into match_events(cod_event,description)
values(128,'EVENT_CAPTURE');  -- This event represents a capture

insert into match_events(cod_event,description)
values(256,'EVENT_CASTLEK');  -- This event represents a short castle

insert into match_events(cod_event,description)
values(512,'EVENT_CASTLEQ');  -- This event represents a long castle

insert into match_events(cod_event,description)
values(1024,'EVENT_PAWN1');  -- This event represents a pawn. In a move there can be up to three pieces involved, this event allows to identify one of them.

insert into match_events(cod_event,description)
values(2048,'EVENT_WBISHOP1');  -- This event represents a light-square bishop. In a move there can be up to three pieces involved, this event allows to identify one of them.

insert into match_events(cod_event,description)
values(4096,'EVENT_KNIGHT1'); -- This event represents a knight. In a move there can be up to three pieces involved, this event allows to identify which piece is moving.

insert into match_events(cod_event,description)
values(8192,'EVENT_ROOK1');  -- This event represents a rook. In a move there can be up to three pieces involved, this event allows to identify which piece is moving.

insert into match_events(cod_event,description)
values(16384,'EVENT_QUEEN1');  -- This event represents a queen. In a move there can be up to three pieces involved, this event allows to identify which piece is moving.

insert into match_events(cod_event,description)
values(32768,'EVENT_KING1'); -- This event represents a king. In a move there can be up to three pieces involved, this event allows to identify which piece is moving.

insert into match_events(cod_event,description)
values(65536,'EVENT_PAWN2'); -- This event represents a pawn. In a move there can be up to three pieces involved, this event allows to identify which piece is captured.

insert into match_events(cod_event,description)
values(131072,'EVENT_WBISHOP2');  -- This event represents a light-square bishop. In a move there can be up to three pieces involved, this event allows to identify which piece is captured.

insert into match_events(cod_event,description)
values(262144,'EVENT_KNIGHT2');  -- This event represents a knight. In a move there can be up to three pieces involved, this event allows to identify which piece is captured.

insert into match_events(cod_event,description)
values(524288,'EVENT_ROOK2');  -- This event represents a rook. In a move there can be up to three pieces involved, this event allows to identify which piece is captured.

insert into match_events(cod_event,description)
values(1048576,'EVENT_QUEEN2');  -- This event represents a queen. In a move there can be up to three pieces involved, this event allows to identify which piece is captured.

insert into match_events(cod_event,description)
values(2097152,'EVENT_MOVE');  -- This event represents a move.

insert into match_events(cod_event,description)
values(4194304,'EVENT_BBISHOP1');  -- This event represents a dark-square bishop. In a move there can be up to three pieces involved, this event allows to identify which piece moves.

insert into match_events(cod_event,description)
values(8388608,'EVENT_BBISHOP2');  -- This event represents a dark-square bishop. In a move there can be up to three pieces involved, this event allows to identify which piece is captured.

insert into match_events(cod_event,description)
values(16777216,'EVENT_BISHOP3');  -- This event represents a generic bishop. In a move there can be up to three pieces involved, this event allows to identify the piece to which a pawn promotes.

insert into match_events(cod_event,description)
values(33554432,'EVENT_KNIGHT3');  -- This event represents a generic knight. In a move there can be up to three pieces involved, this event allows to identify the piece to which a pawn promotes.

insert into match_events(cod_event,description)
values(67108864,'EVENT_ROOK3');  -- This event represents a generic rook. In a move there can be up to three pieces involved, this event allows to identify the piece to which a pawn promotes.

insert into match_events(cod_event,description)
values(134217728,'EVENT_QUEEN3');  -- This event represents a generic queen. In a move there can be up to three pieces involved, this event allows to identify the piece to which a pawn promotes.

/*
The keywords table contains the keywords that are used to classify positions, statistics, and games.
*/
create table keywords(
    COD_KEYWORD NUMBER(32) default seq_keywords.nextval NOT NULL,            -- Surrogate key
    KEYWORD VARCHAR2(250) NOT NULL,             -- This is the name of the keyword
	KEYWORD_TYPE VARCHAR2(3) DEFAULT 'MKW',     -- The type of keyword: MKW = Match Keyword, PKW = Position Keyword, MST = Match Statistic, PST = Position Statistic
    DESCRIPTION VARCHAR2(4000),                 -- This is a description of the keyword.
    CONSTRAINT PK_KEYWORDS
        PRIMARY KEY(COD_KEYWORD),               -- This is the primary key
	CONSTRAINT UK_KEYWORDS
		UNIQUE(KEYWORD,KEYWORD_TYPE)            -- This is a unique key
);	
/*
The keyword_values table stores the values of the keywords.
*/
create table keyword_values(
	COD_VALUE NUMBER(32) DEFAULT seq_kvalues.NEXTVAL NOT NULL,  		-- Surrogate key
    KW_VALUE VARCHAR2(500),                 -- Keyword value. It can be text or numeric stored as text, depending on the keyword.
    CONSTRAINT PK_KEY_VALUES
        PRIMARY KEY(COD_VALUE)              -- This is the primary key
);
/*
The positions table contains each of the positions of the game, that is, the arrangement of the board at each move, and some basic counts to optimize queries.
Each match contains one of these positions for each of the players' moves.
The board is saved using the following format:
64 characters, one for each square of the board, from left to right and from top to bottom.
- 0 = empty square
- p = black pawn
- k = black king
- q = black queen
- r = black tower
- n = black horse
- b = black bishop
- P = white pawn
- K = white king
- Q = white queen
- R = white rook
- N = white horse
- B = white bishop
*/
create table positions(
    cod_position number(32) default seq_positions.nextval NOT NULL,       -- Surrogate key
    board varchar2(64) not null,            -- This is the board. This is an example: 00b000000pP00QN00n0p000q00bPp00000PkPp0p0P000P0000K00P0000000000. To request the draw_image action to generate an image of this board, you should use this drawerId: 
    black_pawns number(1) default 0,        -- number of black pawns
    white_pawns number(1) default 0,        -- number of white pawns
    black_rooks number(1) default 0,
    white_rooks number(1) default 0,        -- number of white rooks
    black_bishops number(1) default 0,      -- number of black bishops
    white_bishops number(1) default 0,      -- number of white bishops
    black_knights number(1) default 0,
    white_knights number(1) default 0,      -- number of white knights
    black_queens number(1) default 0,       -- número de reinas negras
    white_queens number(1) default 0,       -- number of white queens
    sts_date date,                          -- Date of statistical collection
    constraint PK_POSITIONS
        PRIMARY KEY(COD_POSITION),          -- This is the primary key
	constraint UK_BOARD                     -- This is a unique key to avoid duplicate positions, different games can reference the same position if they have passed through it.
		UNIQUE(BOARD)
);
/*
The position_statistics table contains some position statistics counts.
The statistics are identified by keywords. There are two for the positions:
COD_KEYWORD = 3, count of games in which this position has appeared.
COD_KEYWORD = 4, account of the times this position has appeared.
*/
create table position_statistics(
    COD_KEYWORD NUMBER(32) NOT NULL,        -- Keyword code
    COD_POSITION NUMBER(32) NOT NULL,       -- Position code
    ST_VALUE NUMBER,                        -- Value of the statistic
    CONSTRAINT PK_POSSTS
        PRIMARY KEY(COD_KEYWORD,COD_POSITION),      -- This is the primary key
    CONSTRAINT FK_PS_POSITION
        FOREIGN KEY(COD_POSITION)
    REFERENCES positions(COD_POSITION),         -- This is the foreign key with the positions table
        CONSTRAINT FK_PS_KEYWORD
    FOREIGN KEY(COD_KEYWORD)
        REFERENCES keywords(COD_KEYWORD)            -- This is the foreign key with the keywords table
);
/*
The table matches contains a record for each game.
The games can be only partial. They may not have finished, or they may start from any move. That depends on how they have been recorded with the program. In general, they are complete, but not necessarily.
The table contains some basic data of the game, such as the date, the players, the result, the number of moves, the number of moves, etc.
*/
create table matches(
    COD_MATCH NUMBER(32) default seq_matches.nextval NOT NULL,              -- Surrogate key
    INITIAL_POSITION NUMBER(32) NOT NULL,       -- The code of the position where the game starts (table positions)
    MATCH_DESCRIPTION VARCHAR2(1000),           -- Description of the match, it is a free text field
    MATCH_DATE VARCHAR2(30),                    -- The date of the match, in text format. The format is usually yyyy/mm/dd or yyyy.mm.dd. If the month or day is unknown, ?? is used.
    WHITE VARCHAR2(100),                        -- The name of the player with white pieces. Even if it is the same player, the name can vary, using first name, first and last name, uppercase, lowercase, etc. Always search using like, wildcards, and lower.
    BLACK VARCHAR2(100),                        -- The name of the player with black pieces. Even if it's the same player, the name can vary, using first name, first and last name, uppercase, lowercase, etc. Always search using like, wildcards, and lower.
    RESULT NUMBER(1) NOT NULL,                  -- Result expressed as a number: 1 = white victory, 2 = black victory, 3 = draw, 0 = game not finished
    RESULT_TEXT VARCHAR2(8) NOT NULL,           -- Result expressed as text: '1-0' = white victory, '0-1' = black victory, '1/2-1/2' = draw, '*' = game not finished
    MOVE_COUNT NUMBER(3) NOT NULL,              -- Count of partial moves, that is, the number of white moves + the number of black moves
    FULLMOVE_COUNT NUMBER(3) NOT NULL,          -- Full move count, i.e., the number of pairs of white-black moves
    sts_date date,                              -- Date of statistics collection
    creation_date date default sysdate,         -- Creation date of the record
    sha256 RAW(32),							-- checksum SHA-256 of all the match positions, to avoid insert duplicates (this is an option, users could want to allow duplicates). varbinary(32) in SQL Server and MySQL
    CONSTRAINT PK_MATCHES
        PRIMARY KEY(COD_MATCH),                 -- This is the primary key
    CONSTRAINT FK_INITIAL_POSITION
        FOREIGN KEY(INITIAL_POSITION)
    REFERENCES positions(COD_POSITION)      -- This is the foreign key with the table positions
);
/*
The table match_statistics contains the statistics of the matches.
The keywords for the starting statistics are the following:
- COD_KEYWORD = 29: 	White cheek account
- COD_KEYWORD = 30:	Multiple white check accounts
- COD_KEYWORD = 31:	Count of discovered checks by white
- COD_KEYWORD = 32:   Black's check count
- COD_KEYWORD = 33:   Number of black's multiple checks
- COD_KEYWORD = 34:   Discovered check count for black
- COD_KEYWORD = 35:   Total checkmate count
- COD_KEYWORD = 36:   Total Multiple Checks Account
- COD_KEYWORD = 37:	Total number of discovered checks
*/
create table match_statistics(
    COD_KEYWORD NUMBER(32) NOT NULL,        -- Keyword code
    COD_MATCH NUMBER(32) NOT NULL,        -- Match code
    ST_VALUE NUMBER,                        -- Value of the statistic
    CONSTRAINT PK_MATCHSTS
        PRIMARY KEY(COD_KEYWORD,COD_MATCH), -- This is the primary key
    CONSTRAINT FK_MS_MATCH
        FOREIGN KEY(COD_MATCH)
        REFERENCES matches(COD_MATCH),      -- This is the foreign key with the table matches
    CONSTRAINT FK_MS_KEYWORD
        FOREIGN KEY(COD_KEYWORD) -- This is the foreign key with the keywords table
        REFERENCES KEYWORDS(COD_KEYWORD)
);
/*
The match_positions table relates the positions with the matches.
*/
create table match_positions (
    COD_MATCH NUMBER(32) NOT NULL,          -- Match code
    COD_POSITION NUMBER(32) NOT NULL,       -- Position code
    POSITION_ORDER NUMBER(3) NOT NULL,      -- Order of the position in the game
    POSITION_EVENTS NUMBER(16) default 0,   -- Sum of events produced to move to this position from the previous one. The event code is always a sum of values (pieces involved, movement, etc.), so if a specific one is sought, the BITAND function (Oracle) or the & operator (SQL Server, MySQL) must be used. If the server is unknown, it must be asked before answering. There is no need to cross with match_events unless you want to break down the names of the events.
    SCORE number(11,10),                    -- Score of the position. It is usually empty, that is, at 0.
    sts_date date,                          -- Date of statistics collection
    CONSTRAINT PK_MATCH_POSITIONS
        PRIMARY KEY(COD_MATCH,COD_POSITION,POSITION_ORDER),         -- This is the primary key
    CONSTRAINT FK_MP_POSITION
        FOREIGN KEY(COD_POSITION)
        REFERENCES positions(COD_POSITION),                         -- This is the foreign key with the table positions
    CONSTRAINT FK_MP_MATCH
        FOREIGN KEY(COD_MATCH)
        REFERENCES matches(COD_MATCH) -- This is the foreign key with the table matches
);  
/*
The table match_moves contains the movements of the games.
This table allows obtaining information about the movements that have been made to go from one position to another.
*/
create table match_moves(
    COD_MATCH NUMBER(32) NOT NULL,              -- Game code
    FROM_POSITION NUMBER(32) NOT NULL,          -- Position code from which it starts
    TO_POSITION NUMBER(32) NOT NULL,            -- Code of the position to which it arrives
    MOVE_ORDER NUMBER(3) NOT NULL,              -- It is the number of the move. The first move of the white pieces is 1, the first move of the black pieces is 2, the second move of the white pieces is 3, etc.
    MOVE_NUMBER NUMBER(3) NOT NULL,             -- It is the number of the move. The first move of the white pieces is 1, the first move of the black pieces is 1, the second move of the white pieces is 2, etc.
    MOVE_PLAYER NUMBER(1) NOT NULL,             -- 0 = white, 1 = black
    MOVE_EVENTS NUMBER(16) default 0,           -- Sum of events produced in this move. It is not necessary to cross with match_events, unless you want to break down the names of the events. The event code is always a sum of values (pieces involved, movement, etc.), so if one is looking for a specific one, one must use the BITAND function (Oracle) or the & operator (SQL Server, MySQL). If the server is unknown, it should be asked before answering.
    MOVE_FROM NUMBER(2) NOT NULL,               -- Number of the origin square, is a number from 0 to 63 (column - 1) + (8 * (row - 1))
    MOVE_TO NUMBER(2) NOT NULL,                 -- Destination square number, it is a number from 0 to 63 (column - 1) + (8 * (row - 1))
    MOVE_AN_TEXT VARCHAR2(10),                  -- Notación algebraica normal. Texto con el que se puede representar el movimiento al imprimir la partida.
    COMMENTS NUMBER(1) default 0,               -- 0 = the movement has no comments, 1 = the movement has comments in the table move_comments
    SCORE number(11,10),                        -- Position score. It is usually empty, meaning 0.
    CONSTRAINT PK_MATCH_MOVES
        PRIMARY KEY(COD_MATCH,MOVE_ORDER),                                      -- This is the primary key
    CONSTRAINT UK_MOVEPLAYER
        UNIQUE(COD_MATCH,MOVE_NUMBER,MOVE_PLAYER),                              -- this is a unique key
    CONSTRAINT FK_FROM_POSITION
        FOREIGN KEY(FROM_POSITION)
        REFERENCES positions(COD_POSITION),                                     -- This is the foreign key with the positions table
    CONSTRAINT FK_MMT_POSITION
        FOREIGN KEY(COD_MATCH,TO_POSITION,MOVE_ORDER)
        REFERENCES match_positions(COD_MATCH,COD_POSITION,POSITION_ORDER),      -- This is the foreign key with the table match_positions
    CONSTRAINT FK_MM_MATCH
        FOREIGN KEY(COD_MATCH)
        REFERENCES matches(COD_MATCH)                                           -- This is the foreign key with the table matches
);
/*
The table match_keywords contains the keyword values of the matches.
This is a table of the starting keyword keys and their names:
45 Opening
46 Annotator
49 WhiteFideId
50	BlackFideId
51 WhiteClock
52 BlackClock
53 BlackTitle
54 Variation
55 WhiteTitle
48 Opening
47 Plycount
5 Event
6 Site
7 Date
8 Round
9 White
10 Black
11	Result
12	EventDate
13 SetUp
14 PlyCount
15 FEN
16 ECO
17 EventType
18 EventRounds
19	EventCountry
20 Source
21	SourceDate
22 WhiteElo
23 BlackElo
24	EventCategory
25 WhiteTeam
26 BlackTeam
27	WhiteTeamCountry
28	BlackTeamCountry
*/
create table match_keywords(
    COD_KEYWORD NUMBER(32) NOT NULL,            -- Code of the keyword
    COD_VALUE NUMBER(32) NOT NULL,              -- Code of the keyword value
    COD_MATCH NUMBER(32) NOT NULL,              -- Match code
    CONSTRAINT PK_MKEYWORDS
        PRIMARY KEY(COD_KEYWORD, COD_MATCH),    -- This is the primary key
    CONSTRAINT FK_MKV_KEYWORD
        FOREIGN KEY(COD_KEYWORD)
        REFERENCES KEYWORDS(COD_KEYWORD),       -- This is the foreign key with the keywords table
    CONSTRAINT FK_MKV_VALUE
        FOREIGN KEY(COD_VALUE)
        REFERENCES KEYWORD_VALUES(COD_VALUE),   -- This is the foreign key with the table keyword_values
    CONSTRAINT FK_MKV_MATCH
        FOREIGN KEY(COD_MATCH)
        REFERENCES MATCHES(COD_MATCH)           -- This is the foreign key with the table matches
);
/*
The table move_comments contains comments about some movements.
*/
create table move_comments(
    COD_COMMENT NUMBER(32) default seq_comments.nextval NOT NULL,        -- Surrogate key
    COD_MATCH NUMBER(32) NOT NULL,          -- Match code
    MOVE_ORDER NUMBER(3) NOT NULL,          -- Move order in the game
    COMMENT_TEXT VARCHAR2(4000) NOT NULL,   -- Comment text
    COMMENT_ORDER NUMBER(3) DEFAULT 0,      -- Comment order in the transaction. There can be multiple comments for the same transaction.
    CONSTRAINT PK_MOVE_COMMENTS
        PRIMARY KEY(COD_COMMENT),                       -- This is the primary key
    CONSTRAINT FK_MOVE_COMMENT
        FOREIGN KEY(COD_MATCH,MOVE_ORDER)
    REFERENCES match_moves(COD_MATCH,MOVE_ORDER)    -- This is the foreign key with the table match_moves
);
-- Those are some useful indexes to optimize queries.
create index IX_MATCHMOVECOUNT on matches(move_count);

create index IX_POSPZCOUNT on positions(white_queens,black_queens,white_rooks,black_rooks,white_bishops,black_bishops,white_knights,black_knights,white_pawns,black_pawns);

create index IX_POSITIONSTATS on positions(0,sts_date);  -- The 0 is a dummy column to allow creating an index that includes null sts_date values.

create index IX_KEYVALUE on keyword_values(cod_value,kw_value);

CREATE INDEX ix_matches_sha256 ON matches(sha256);

commit;

-- This view allows retrieving the game data together with all of its positions, including the initial position of the game.
create or replace view VW_MATCH_POSITIONS AS
select m.COD_MATCH,m.INITIAL_POSITION,pi.board as initial_board,pi.black_pawns as initial_black_pawns,pi.white_pawns as initial_white_pawns,pi.black_rooks as initial_black_rooks,
pi.white_rooks as initial_white_rooks,pi.black_bishops as initial_black_bishops,pi.white_bishops as initial_white_bishops,pi.black_knights as initial_black_knights,
pi.white_knights as initial_white_knights,pi.black_queens as initial_black_queens,pi.white_queens as initial_white_queens,m.MATCH_DESCRIPTION,m.MATCH_DATE,m.WHITE,m.BLACK,
m.RESULT,m.RESULT_TEXT,m.MOVE_COUNT,m.FULLMOVE_COUNT,mp.POSITION_ORDER,mp.POSITION_EVENTS,mp.SCORE,mpn.board,mpn.black_pawns,mpn.white_pawns,mpn.black_rooks,
mpn.white_rooks,mpn.black_bishops,mpn.white_bishops,mpn.black_knights,mpn.white_knights,mpn.black_queens,mpn.white_queens
from matches m join positions pi on m.initial_position = pi.cod_position
join match_positions mp on mp.cod_match = m.cod_match join positions mpn on mp.cod_position = mpn.cod_position;

-- This view allows retrieving the game data together with all its moves and positions, including the initial game position as well as the starting and ending positions of each move.
create or replace view VW_MATCH_MOVES AS
select m.COD_MATCH,m.INITIAL_POSITION,pi.board as initial_board,pi.black_pawns as initial_black_pawns,pi.white_pawns as initial_white_pawns,pi.black_rooks as initial_black_rooks,
pi.white_rooks as initial_white_rooks,pi.black_bishops as initial_black_bishops,pi.white_bishops as initial_white_bishops,pi.black_knights as initial_black_knights,
pi.white_knights as initial_white_knights,pi.black_queens as initial_black_queens,pi.white_queens as initial_white_queens,m.MATCH_DESCRIPTION,m.MATCH_DATE,m.WHITE,m.BLACK,
m.RESULT,m.RESULT_TEXT,m.MOVE_COUNT,m.FULLMOVE_COUNT,mm.MOVE_ORDER,mm.MOVE_NUMBER,mm.MOVE_PLAYER,mm.MOVE_EVENTS,mm.MOVE_AN_TEXT,mm.COMMENTS,mm.SCORE,mm.MOVE_FROM,mm.FROM_POSITION,fp.board as from_board,fp.black_pawns as from_black_pawns,fp.white_pawns as from_white_pawns,fp.black_rooks as from_black_rooks,
fp.white_rooks as from_white_rooks,fp.black_bishops as from_black_bishops,fp.white_bishops as from_white_bishops,fp.black_knights as from_black_knights,fp.white_knights as from_white_knights,fp.black_queens as from_black_queens,fp.white_queens as from_white_queens,
mm.MOVE_TO,mm.TO_POSITION,tp.board as to_board,tp.black_pawns as to_black_pawns,tp.white_pawns as to_white_pawns,tp.black_rooks as to_black_rooks,
tp.white_rooks as to_white_rooks,tp.black_bishops as to_black_bishops,tp.white_bishops as to_white_bishops,tp.black_knights as to_black_knights,tp.white_knights as to_white_knights,tp.black_queens as to_black_queens,tp.white_queens as to_white_queens
from matches m join positions pi on m.initial_position = pi.cod_position
join match_moves mm on mm.cod_match = m.cod_match join positions fp on mm.from_position = fp.cod_position join positions tp on mm.to_position = tp.cod_position;

-- This view allows retrieving the game data together with its keywords and statistics.

create or replace view VW_MATCH_KEYWORDS as
select m.COD_MATCH,m.MATCH_DESCRIPTION,m.MATCH_DATE,m.WHITE,m.BLACK,m.RESULT,m.RESULT_TEXT,m.MOVE_COUNT,m.FULLMOVE_COUNT,mk.COD_KEYWORD,k.KEYWORD,kv.kw_value,ks.KEYWORD statistic,ms.st_value 
from matches m join match_keywords mk on mk.COD_MATCH = m.COD_MATCH
join keywords k on k.COD_KEYWORD = mk.COD_KEYWORD
join keyword_values kv on kv.COD_VALUE = mk.COD_VALUE
join match_statistics ms on ms.COD_MATCH = m.COD_MATCH
join keywords ks on ks.COD_KEYWORD = ms.COD_KEYWORD;

-- Model usage tracking table
create table token_usage(
    COD_USAGE NUMBER(32) DEFAULT seq_token_usage.nextval not null primary key,  -- Surrogate key
    USAGE_DATE date not null,                           -- Date of usage
	PLAYER_NAME varchar2(50) not null,                  -- Name of the assistant
    MODEL_NAME varchar2(50) not null,                   -- LLM model name
    TEMPERATURE NUMBER(3,2),                            -- Temperature parameter value
	TOP_P NUMBER(3,2),                                  -- Top_p parameter value    
	INPUT_TOKENS NUMBER(8) default 0,                   -- Number of input tokens
	OUTPUT_TOKENS NUMBER(8) default 0,                  -- Number of output tokens
	AUDIO_INPUT_TOKENS NUMBER(8) default 0,             -- Number of audio input tokens
	AUDIO_OUTPUT_TOKENS NUMBER(8) default 0,            -- Number of audio output tokens
	REASONING_TOKENS NUMBER(8) default 0,               -- Number of reasoning tokens
	INPUT_CACHED_TOKENS NUMBER(8) default 0,            -- Number of input cached tokens
	INPUT_IMAGE_TOKENS NUMBER(8) default 0,             -- Number of input image tokens
	INPUT_TEXT_TOKENS NUMBER(8) default 0               -- Number of input text tokens
);
