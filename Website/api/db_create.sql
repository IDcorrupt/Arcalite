CREATE DATABASE arcalite CHARACTER SET utf8;
USE arcalite;

CREATE TABLE profile (
  id INT PRIMARY KEY AUTO_INCREMENT,
  username VARCHAR(64),
  password VARCHAR(128),
  played TIME DEFAULT 0,
  email VARCHAR(128),
  deletedAt DATETIME DEFAULT NULL
);

CREATE TABLE achievement (
  id INT PRIMARY KEY AUTO_INCREMENT
);

CREATE TABLE proach (
  profileid INT,
  achievementid INT,
  CONSTRAINT pk_proach PRIMARY KEY (profileid, achievementid),
  CONSTRAINT fk_proach_profile FOREIGN KEY (profileid) REFERENCES profile(id),
  CONSTRAINT fk_proach_achievement FOREIGN KEY (achievementid) REFERENCES achievement(id)
);

CREATE TABLE avatar (
  id INT PRIMARY KEY AUTO_INCREMENT,
  splash VARCHAR(128),
  image VARCHAR(128)
);

CREATE TABLE level (
  id INT PRIMARY KEY AUTO_INCREMENT,
  image VARCHAR(128)
);

CREATE TABLE player (
  id INT PRIMARY KEY AUTO_INCREMENT,
  name VARCHAR(64),
  hp INT,
  mp INT,
  profileid INT,
  avatarid INT,
  levelid INT,
  playtime TIME DEFAULT 0,
  CONSTRAINT fk_player_avatar FOREIGN KEY (avatarid) REFERENCES avatar(id),
  CONSTRAINT fk_player_level FOREIGN KEY (levelid) REFERENCES level(id),
  CONSTRAINT fk_player_profile FOREIGN KEY (profileid) REFERENCES profile(id)
);
CREATE TABLE enemy (
  id INT PRIMARY KEY AUTO_INCREMENT,
  hp INT,
  image VARCHAR(128)
);

CREATE TABLE enemplay (
  enemyid INT,
  playerid INT,
  CONSTRAINT pk_enemplay PRIMARY KEY (enemyid, playerid),
  CONSTRAINT fk_enemplay_enemy FOREIGN KEY (enemyid) REFERENCES enemy(id),
  CONSTRAINT fk_enemplay_player FOREIGN KEY (playerid) REFERENCES player(id)
);

CREATE TABLE item (
  id INT PRIMARY KEY AUTO_INCREMENT,
  image VARCHAR(128)
);

CREATE TABLE itemplay (
  itemid INT,
  playerid INT,
  CONSTRAINT pk_itemplay PRIMARY KEY (itemid, playerid),
  CONSTRAINT fk_itemplay_item FOREIGN KEY (itemid) REFERENCES item(id),
  CONSTRAINT fk_itemplay_player FOREIGN KEY (playerid) REFERENCES player(id)
);

CREATE TABLE lang (
  id INT PRIMARY KEY AUTO_INCREMENT,
  name VARCHAR(64)
);

CREATE TABLE achdesc (
  achievementid INT,
  languageid INT,
  name VARCHAR(255),
  description TEXT,
  CONSTRAINT pk_achdesc PRIMARY KEY (achievementid, languageid),
  CONSTRAINT fk_achdesc_achievement FOREIGN KEY (achievementid) REFERENCES achievement(id),
  CONSTRAINT fk_achdesc_languages FOREIGN KEY (languageid) REFERENCES lang(id)
);

CREATE TABLE avatardesc (
  avatarid INT,
  languageid INT,
  name VARCHAR(255),
  description TEXT,
  CONSTRAINT pk_avatardesc PRIMARY KEY (avatarid, languageid),
  CONSTRAINT fk_avatardesc_avatar FOREIGN KEY (avatarid) REFERENCES avatar(id),
  CONSTRAINT fk_avatardesc_languages FOREIGN KEY (languageid) REFERENCES lang(id)
);

CREATE TABLE itemdesc (
  itemid INT,
  languageid INT,
  name VARCHAR(255),
  description TEXT,
  CONSTRAINT pk_itemdesc PRIMARY KEY (itemid, languageid),
  CONSTRAINT fk_itemdesc_item FOREIGN KEY (itemid) REFERENCES item(id),
  CONSTRAINT fk_itemdesc_languages FOREIGN KEY (languageid) REFERENCES lang(id)
);

CREATE TABLE enemydesc (
  enemyid INT,
  languageid INT,
  name VARCHAR(255),
  description TEXT,
  CONSTRAINT pk_enemydesc PRIMARY KEY (enemyid, languageid),
  CONSTRAINT fk_enemydesc_item FOREIGN KEY (enemyid) REFERENCES enemy(id),
  CONSTRAINT fk_enemydesc_languages FOREIGN KEY (languageid) REFERENCES lang(id)
);

CREATE TABLE leveldesc (
  levelid INT,
  languageid INT,
  name VARCHAR(255),
  description TEXT,
  CONSTRAINT pk_leveldesc PRIMARY KEY (levelid, languageid),
  CONSTRAINT fk_leveldesc_level FOREIGN KEY (levelid) REFERENCES level(id),
  CONSTRAINT fk_leveldesc_languages FOREIGN KEY (languageid) REFERENCES lang(id)
);

CREATE TABLE saves (
  playerid INT,
  time DATETIME DEFAULT CURRENT_TIMESTAMP,
  save LONGBLOB,
  CONSTRAINT pk_saves PRIMARY KEY (playerid, time),
  CONSTRAINT fk_saves_player FOREIGN KEY (playerid) REFERENCES player(id)
);