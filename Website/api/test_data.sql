REPLACE INTO lang (id, name) VALUES 
(1, 'Magyar'), 
(2, 'English');

--ENEMIES, ITEMS, LEVELS
--TODO: AT LEAST DOING THE FIRST COUPLE LEVELS (UNDEAD)

INSERT INTO enemy (id, hp, image) VALUES 
();

INSERT INTO enemydesc (enemyid, languageid, name, description) VALUES 
();

INSERT INTO item (id image) VALUES
();

INSERT INTO itemdesc (itemid, languageid, name, description) VALUES 
();

REPLACE INTO level (id, image) VALUES
(1, 'Kep1.png'),
(2, 'Kep2.png'),
(3, 'Kep3.png');

INSERT INTO leveldesc (levelid, languageid, name, description) VALUES 
();

--PROFILES, AVATARS, CHARACTERS

REPLACE INTO profile (id, username, password, played, email) VALUES
(1, "haro", PASSWORD("haro"), '00:00', "haro@gmail.com"),
(2, "feri", PASSWORD("Ferike123"), '00:00', "ferenc001_@gmail.com"),
(3, "laci", PASSWORD("LaszloLaszlo"), '05:19', "zeczi.laszlo@gmail.com"),
(4, "alcsi", PASSWORD("aladar001"), '00:24:32', "aladar1@gmail.com"),
(5, "vinko08", PASSWORD("vince2008"), '00:13:13', "vince2008@gmail.com");

REPLACE INTO avatar (id, image) VALUES
(1, "kep1.png"),
(2, "kep1.png"),
(3, "forg.png");

REPLACE INTO avatardesc (avatarid, languageid, name, description) VALUES
(1, 1, "Első avatár", "Alapértelmezett avatár"),
(2, 1, "Második avatár", "Alapértelmezett avatár"),
(3, 1, "Béak", "Brek-brek"),
(1, 2, "First avatar", "Default avatar"),
(2, 2, "Second avatar", "Default avatar"),
(3, 2, "Forg", "Ribbit-ribbit");

REPLACE INTO player (id, name, hp, profileid, avatarid, levelid, playtime) VALUES
(1, "Haró", 100, 1, 1, 1, "00:00"),
(2, "Harcos", 100, 2, 2, 1, "00:00"),
(3, "Toldi Miklós", 100, 3, 2, 1, "01:33"),
(4, "Ferg", 100, 3, 3, 2, "03:46");

INSERT INTO enemplay (playerid, enemyid) VALUES 
();

INSERT INTO itemplay (playerid, itemid) VALUES
();

--ACHIEVEMENTS (OPTIONAL)

INSERT INTO achievement (id) VALUES ();

INSERT INTO achdesc (achievementid, languageid, name, description) VALUES
();

INSERT INTO proach (profileid, achievementid) VALUES 
();