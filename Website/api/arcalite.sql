-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Már 20. 11:04
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.0.30

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `arcalite`
--
CREATE DATABASE IF NOT EXISTS `arcalite` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `arcalite`;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `achdesc`
--

DROP TABLE IF EXISTS `achdesc`;
CREATE TABLE `achdesc` (
  `achievementid` int(11) NOT NULL,
  `languageid` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `achdesc`
--

REPLACE INTO `achdesc` (`achievementid`, `languageid`, `name`, `description`) VALUES
(1, 1, 'Valahol el kell kezdeni', 'Öld meg az első szörnyed.'),
(1, 2, 'You gotta start somewhere', 'Kill your first monster.'),
(2, 1, 'Na ez mi?', 'Vedd fel az első tárgyad.'),
(2, 2, 'What\'s this now?', 'Pick up your first item.'),
(3, 1, 'Halhatatlan', 'Érj el N életpontot.'),
(3, 2, 'Immortal', 'Reach N healthpoints.'),
(4, 1, 'Varázspuska', 'Érj el N manapontot.'),
(4, 2, 'Magic Minigun', 'Reach N manapoints.'),
(5, 1, 'AU!', 'Érj el N sebzést egy ütésből.'),
(5, 2, 'OW!', 'Reach N attack damage.'),
(6, 1, 'Vége lesz valaha?', 'Vidd végig az első futamodat.'),
(6, 2, 'Will it ever end?', 'Finish your first run.');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `achievement`
--

DROP TABLE IF EXISTS `achievement`;
CREATE TABLE `achievement` (
  `id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `achievement`
--

REPLACE INTO `achievement` (`id`) VALUES
(1),
(2),
(3),
(4),
(5),
(6);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `avatar`
--

DROP TABLE IF EXISTS `avatar`;
CREATE TABLE `avatar` (
  `id` int(11) NOT NULL,
  `image` varchar(128) DEFAULT NULL,
  `splash` varchar(128) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `avatar`
--

REPLACE INTO `avatar` (`id`, `image`, `splash`) VALUES
(1, 'maleros.png', 'maleros_portrait.png');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `avatardesc`
--

DROP TABLE IF EXISTS `avatardesc`;
CREATE TABLE `avatardesc` (
  `avatarid` int(11) NOT NULL,
  `languageid` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `avatardesc`
--

REPLACE INTO `avatardesc` (`avatarid`, `languageid`, `name`, `description`) VALUES
(1, 1, 'Maleros', 'Maleros egy tiefling mágus, aki képes a kézmozdulataival varázsolni.  '),
(1, 2, 'Maleros', 'Maleros is a tiefling sorcerer, able to emit magic from their hands. ');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `enemplay`
--

DROP TABLE IF EXISTS `enemplay`;
CREATE TABLE `enemplay` (
  `enemyid` int(11) NOT NULL,
  `playerid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `enemplay`
--

REPLACE INTO `enemplay` (`enemyid`, `playerid`) VALUES
(1, 4),
(1, 5),
(1, 6),
(2, 4),
(2, 5),
(2, 6),
(3, 4),
(3, 5),
(4, 4);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `enemy`
--

DROP TABLE IF EXISTS `enemy`;
CREATE TABLE `enemy` (
  `id` int(11) NOT NULL,
  `hp` int(11) DEFAULT NULL,
  `image` varchar(128) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `enemy`
--

REPLACE INTO `enemy` (`id`, `hp`, `image`) VALUES
(1, NULL, 'skeleton.png'),
(2, NULL, 'witch.png'),
(3, NULL, 'cyclops.png'),
(4, NULL, 'lich.png'),
(5, NULL, 'undead_boss.png');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `enemydesc`
--

DROP TABLE IF EXISTS `enemydesc`;
CREATE TABLE `enemydesc` (
  `enemyid` int(11) NOT NULL,
  `languageid` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `enemydesc`
--

REPLACE INTO `enemydesc` (`enemyid`, `languageid`, `name`, `description`) VALUES
(1, 1, 'Csontváz harcos', 'Alapszintű közelharci szörny. Közel jön, megüt.'),
(1, 2, 'Skeleton Warrior', 'Basic melee monster. It hits when it gets close to the player.'),
(2, 1, 'Boszorkány', 'Alapszintű mágus.'),
(2, 2, 'Witch', 'Basic caster.'),
(3, 1, 'Küklopsz', 'Elit közelharci szörny. Az átlagnál nagyobbat üt.'),
(3, 2, 'Cyclops', 'Elite melee monster. It hits harder than the average. '),
(4, 1, 'Holt mágus', 'Elit mágus. '),
(4, 2, 'Lich', 'Elite caster.'),
(5, 1, 'Élőhalott főellenség', '???'),
(5, 2, 'Undead boss', '???');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `item`
--

DROP TABLE IF EXISTS `item`;
CREATE TABLE `item` (
  `id` int(11) NOT NULL,
  `image` varchar(128) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `item`
--

REPLACE INTO `item` (`id`, `image`) VALUES
(1, 'necklace.png'),
(2, 'orichalcum_core.png');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `itemdesc`
--

DROP TABLE IF EXISTS `itemdesc`;
CREATE TABLE `itemdesc` (
  `itemid` int(11) NOT NULL,
  `languageid` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `itemdesc`
--

REPLACE INTO `itemdesc` (`itemid`, `languageid`, `name`, `description`) VALUES
(1, 1, 'Nyaklánc', 'Csökkenti a támadási sebesség töltési idejét, de kevésbé pontos.'),
(1, 2, 'Necklace', 'Reduces basic attack speed cooldown, but also makes it less accurate.'),
(2, 1, 'Orichalcum mag', 'Sebezhetetlenné teszi a játékost két másodpercig.'),
(2, 2, 'Orichalcum core', 'Makes the player invulnerable for two seconds.');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `itemplay`
--

DROP TABLE IF EXISTS `itemplay`;
CREATE TABLE `itemplay` (
  `itemid` int(11) NOT NULL,
  `playerid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `itemplay`
--

REPLACE INTO `itemplay` (`itemid`, `playerid`) VALUES
(1, 4),
(1, 5),
(2, 4);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `lang`
--

DROP TABLE IF EXISTS `lang`;
CREATE TABLE `lang` (
  `id` int(11) NOT NULL,
  `name` varchar(64) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `lang`
--

REPLACE INTO `lang` (`id`, `name`) VALUES
(1, 'Magyar'),
(2, 'English');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `level`
--

DROP TABLE IF EXISTS `level`;
CREATE TABLE `level` (
  `id` int(11) NOT NULL,
  `image` varchar(128) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `level`
--

REPLACE INTO `level` (`id`, `image`) VALUES
(1, '1'),
(2, '2'),
(3, '3');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `leveldesc`
--

DROP TABLE IF EXISTS `leveldesc`;
CREATE TABLE `leveldesc` (
  `levelid` int(11) NOT NULL,
  `languageid` int(11) NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `player`
--

DROP TABLE IF EXISTS `player`;
CREATE TABLE `player` (
  `id` int(11) NOT NULL,
  `name` varchar(64) DEFAULT NULL,
  `hp` int(11) DEFAULT NULL,
  `mp` int(11) DEFAULT NULL,
  `profileid` int(11) DEFAULT NULL,
  `avatarid` int(11) DEFAULT NULL,
  `levelid` int(11) DEFAULT NULL,
  `playtime` time DEFAULT '00:00:00'
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `player`
--

REPLACE INTO `player` (`id`, `name`, `hp`, `mp`, `profileid`, `avatarid`, `levelid`, `playtime`) VALUES
(4, 'DinoHunter', 100, 100, 3, 1, 3, '00:19:44'),
(5, 'DinoHuntingXx', 100, 100, 3, 1, 2, '00:05:02'),
(6, 'Kálmán', 100, 100, 1, 1, 3, '00:19:14');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `proach`
--

DROP TABLE IF EXISTS `proach`;
CREATE TABLE `proach` (
  `profileid` int(11) NOT NULL,
  `achievementid` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `proach`
--

REPLACE INTO `proach` (`profileid`, `achievementid`) VALUES
(1, 1),
(1, 2),
(1, 3),
(3, 1);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `profile`
--

DROP TABLE IF EXISTS `profile`;
CREATE TABLE `profile` (
  `id` int(11) NOT NULL,
  `username` varchar(64) DEFAULT NULL,
  `password` varchar(128) DEFAULT NULL,
  `played` time DEFAULT '00:00:00',
  `email` varchar(128) DEFAULT NULL,
  `deletedAt` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- A tábla adatainak kiíratása `profile`
--

REPLACE INTO `profile` (`id`, `username`, `password`, `played`, `email`, `deletedAt`) VALUES
(1, 'Zoli26', '*F2DE9CCD4C692570BBEB3218207B889F3204635C', '00:26:42', 'zoli26@gmail.com', NULL),
(2, 'kovi_', '*D7284B946F7994DE4F5FFE45288F5D1E9157EB68', '00:00:00', 'kovacs.norbi.2004@gmail.com', NULL),
(3, 'DinoHunter2004', '*011CFB74686EDAFBF89D738975DB0FACF2D7D105', '00:19:14', 'nagy.geza@gmail.com', NULL),
(4, 'JancsikT', '*0FBE4427EC561BFB315AA4AC89CEFD52D4AC34A6', '00:00:00', 'jani.t@gmail.com', NULL);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `saves`
--

DROP TABLE IF EXISTS `saves`;
CREATE TABLE `saves` (
  `playerid` int(11) NOT NULL,
  `time` datetime NOT NULL DEFAULT current_timestamp(),
  `save` longblob DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `achdesc`
--
ALTER TABLE `achdesc`
  ADD PRIMARY KEY (`achievementid`,`languageid`),
  ADD KEY `fk_achdesc_languages` (`languageid`);

--
-- A tábla indexei `achievement`
--
ALTER TABLE `achievement`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `avatar`
--
ALTER TABLE `avatar`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `avatardesc`
--
ALTER TABLE `avatardesc`
  ADD PRIMARY KEY (`avatarid`,`languageid`),
  ADD KEY `fk_avatardesc_languages` (`languageid`);

--
-- A tábla indexei `enemplay`
--
ALTER TABLE `enemplay`
  ADD PRIMARY KEY (`enemyid`,`playerid`),
  ADD KEY `fk_enemplay_player` (`playerid`);

--
-- A tábla indexei `enemy`
--
ALTER TABLE `enemy`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `enemydesc`
--
ALTER TABLE `enemydesc`
  ADD PRIMARY KEY (`enemyid`,`languageid`),
  ADD KEY `fk_enemydesc_languages` (`languageid`);

--
-- A tábla indexei `item`
--
ALTER TABLE `item`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `itemdesc`
--
ALTER TABLE `itemdesc`
  ADD PRIMARY KEY (`itemid`,`languageid`),
  ADD KEY `fk_itemdesc_languages` (`languageid`);

--
-- A tábla indexei `itemplay`
--
ALTER TABLE `itemplay`
  ADD PRIMARY KEY (`itemid`,`playerid`),
  ADD KEY `fk_itemplay_player` (`playerid`);

--
-- A tábla indexei `lang`
--
ALTER TABLE `lang`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `level`
--
ALTER TABLE `level`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `leveldesc`
--
ALTER TABLE `leveldesc`
  ADD PRIMARY KEY (`levelid`,`languageid`),
  ADD KEY `fk_leveldesc_languages` (`languageid`);

--
-- A tábla indexei `player`
--
ALTER TABLE `player`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_player_avatar` (`avatarid`),
  ADD KEY `fk_player_level` (`levelid`),
  ADD KEY `fk_player_profile` (`profileid`);

--
-- A tábla indexei `proach`
--
ALTER TABLE `proach`
  ADD PRIMARY KEY (`profileid`,`achievementid`),
  ADD KEY `fk_proach_achievement` (`achievementid`);

--
-- A tábla indexei `profile`
--
ALTER TABLE `profile`
  ADD PRIMARY KEY (`id`);

--
-- A tábla indexei `saves`
--
ALTER TABLE `saves`
  ADD PRIMARY KEY (`playerid`,`time`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `achievement`
--
ALTER TABLE `achievement`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT a táblához `avatar`
--
ALTER TABLE `avatar`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT a táblához `enemy`
--
ALTER TABLE `enemy`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT a táblához `item`
--
ALTER TABLE `item`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `lang`
--
ALTER TABLE `lang`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `level`
--
ALTER TABLE `level`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `player`
--
ALTER TABLE `player`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT a táblához `profile`
--
ALTER TABLE `profile`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `achdesc`
--
ALTER TABLE `achdesc`
  ADD CONSTRAINT `fk_achdesc_achievement` FOREIGN KEY (`achievementid`) REFERENCES `achievement` (`id`),
  ADD CONSTRAINT `fk_achdesc_languages` FOREIGN KEY (`languageid`) REFERENCES `lang` (`id`);

--
-- Megkötések a táblához `avatardesc`
--
ALTER TABLE `avatardesc`
  ADD CONSTRAINT `fk_avatardesc_avatar` FOREIGN KEY (`avatarid`) REFERENCES `avatar` (`id`),
  ADD CONSTRAINT `fk_avatardesc_languages` FOREIGN KEY (`languageid`) REFERENCES `lang` (`id`);

--
-- Megkötések a táblához `enemplay`
--
ALTER TABLE `enemplay`
  ADD CONSTRAINT `fk_enemplay_enemy` FOREIGN KEY (`enemyid`) REFERENCES `enemy` (`id`),
  ADD CONSTRAINT `fk_enemplay_player` FOREIGN KEY (`playerid`) REFERENCES `player` (`id`);

--
-- Megkötések a táblához `enemydesc`
--
ALTER TABLE `enemydesc`
  ADD CONSTRAINT `fk_enemydesc_item` FOREIGN KEY (`enemyid`) REFERENCES `enemy` (`id`),
  ADD CONSTRAINT `fk_enemydesc_languages` FOREIGN KEY (`languageid`) REFERENCES `lang` (`id`);

--
-- Megkötések a táblához `itemdesc`
--
ALTER TABLE `itemdesc`
  ADD CONSTRAINT `fk_itemdesc_item` FOREIGN KEY (`itemid`) REFERENCES `item` (`id`),
  ADD CONSTRAINT `fk_itemdesc_languages` FOREIGN KEY (`languageid`) REFERENCES `lang` (`id`);

--
-- Megkötések a táblához `itemplay`
--
ALTER TABLE `itemplay`
  ADD CONSTRAINT `fk_itemplay_item` FOREIGN KEY (`itemid`) REFERENCES `item` (`id`),
  ADD CONSTRAINT `fk_itemplay_player` FOREIGN KEY (`playerid`) REFERENCES `player` (`id`);

--
-- Megkötések a táblához `leveldesc`
--
ALTER TABLE `leveldesc`
  ADD CONSTRAINT `fk_leveldesc_languages` FOREIGN KEY (`languageid`) REFERENCES `lang` (`id`),
  ADD CONSTRAINT `fk_leveldesc_level` FOREIGN KEY (`levelid`) REFERENCES `level` (`id`);

--
-- Megkötések a táblához `player`
--
ALTER TABLE `player`
  ADD CONSTRAINT `fk_player_avatar` FOREIGN KEY (`avatarid`) REFERENCES `avatar` (`id`),
  ADD CONSTRAINT `fk_player_level` FOREIGN KEY (`levelid`) REFERENCES `level` (`id`),
  ADD CONSTRAINT `fk_player_profile` FOREIGN KEY (`profileid`) REFERENCES `profile` (`id`);

--
-- Megkötések a táblához `proach`
--
ALTER TABLE `proach`
  ADD CONSTRAINT `fk_proach_achievement` FOREIGN KEY (`achievementid`) REFERENCES `achievement` (`id`),
  ADD CONSTRAINT `fk_proach_profile` FOREIGN KEY (`profileid`) REFERENCES `profile` (`id`);

--
-- Megkötések a táblához `saves`
--
ALTER TABLE `saves`
  ADD CONSTRAINT `fk_saves_player` FOREIGN KEY (`playerid`) REFERENCES `player` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
