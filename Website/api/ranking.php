<?php

require_once "config.php";

checkValidity("GET", "type", "langid");

$langid = $_GET['langid'];

switch ($_GET["type"]) {
    case "Profile":
        $sql = "WITH
                achs AS (
                    SELECT profileid AS id, COUNT(achievementid) AS count
                    FROM proach INNER JOIN profile ON proach.profileid = profile.id
                    WHERE profile.deletedAt IS NULL
                    GROUP BY profileid
                ),
                games AS (
                    SELECT profile.id AS id, COUNT(*) AS count
                    FROM profile LEFT JOIN player ON profile.id = player.profileid
                    WHERE player.levelid = (SELECT MAX(id) FROM level) AND profile.deletedAt IS NULL
                    GROUP BY profile.id
                )
                
                SELECT
                    profile.username AS `Felhasználónév`,
                    profile.played AS `Játékidő`,
                    COALESCE(achs.count, 0) AS `Elért_mérföldkövek`,
                    COALESCE(games.count, 0) AS `Végigjátszások`
                FROM 
                    profile 
                    LEFT JOIN achs ON profile.id = achs.id
                    LEFT JOIN games ON profile.id = games.id
                WHERE
                    profile.deletedAt IS NULL;";
        break;
    case "GameThrough":
        $sql = "WITH 
                enemies AS (
                    SELECT playerid AS id, COUNT(enemyid) AS count
                    FROM enemplay
                    GROUP BY playerid
                ),
                items AS (
                    SELECT playerid AS id, COUNT(itemid) AS count
                    FROM itemplay
                    GROUP BY playerid
                )
                
                SELECT
                    player.name AS `Karakter`,
                    profile.username AS `Profil`,
                    avatardesc.name AS `Avatár`,
                    player.playtime AS `Játékidő`,
                    player.levelid AS `Elért_szint`,
                    COALESCE(enemies.count, 0) AS `Felfedezett_ellenfelek`,
                    COALESCE(items.count, 0) AS `Felfedezett_tárgyak`
                FROM 
                    player
                    LEFT JOIN enemies ON enemies.id = player.id
                    LEFT JOIN items ON items.id = player.id
                    INNER JOIN profile ON player.profileid = profile.id
                    INNER JOIN avatar ON player.avatarid = avatar.id
                    INNER JOIN avatardesc ON avatardesc.avatarid = avatar.id
                    INNER JOIN lang ON avatardesc.languageid = lang.id
                WHERE lang.id = $langid AND profile.deletedAt IS NULL
                GROUP BY player.id;";
        break;
    default:
        break;
}

ReturnQuery($sql);