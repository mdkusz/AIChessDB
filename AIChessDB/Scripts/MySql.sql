CREATE TABLESPACE `tbs_chess_pgn` ADD DATAFILE 'tbs_chess_pgn.ibd' ENGINE=InnoDB;

CREATE DATABASE `chess_pgn` CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

CREATE USER 'chess_pgn'@'%' IDENTIFIED BY 'chesspgn' PASSWORD EXPIRE NEVER;

GRANT ALL PRIVILEGES ON `chess_pgn`.* TO 'chess_pgn'@'%';

FLUSH PRIVILEGES;

SET PERSIST event_scheduler = ON;