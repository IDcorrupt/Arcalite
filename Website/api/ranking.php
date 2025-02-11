<?php

require_once "config.php";

checkValidity("GET", "type", "langid");

$langid = $_GET['langid'];

switch ($_GET["type"]) {
    case "Profile":
        $sql = "SELECT
                    profile.username AS `Felhasználónév`,
                    profile.played AS `Játékidő`,
                    COUNT(proach.achievementid) AS `Elért_mérföldkövek`,
                    COUNT(CASE WHEN player.levelid = (SELECT MAX(id) FROM level) THEN player.id END) AS `Végigjátszások`
                FROM 
                    profile 
                    LEFT JOIN proach ON profile.id = proach.profileid
                    LEFT JOIN player ON profile.id = player.profileid
                GROUP BY profile.id;";
        break;
    case "GameThrough":
        $sql = "SELECT
                    player.name AS `Karakter`,
                    profile.username AS `Profil`,
                    avatardesc.name AS `Ąvatár`,
                    player.playtime AS `Játékidő`,
                    player.levelid AS `Elért_szint`,
                    COUNT(enemplay.enemyid) AS `Felfedezett_ellenfelek`,
                    COUNT(itemplay.itemid) AS `Felfedezett_tárgyak`
                FROM 
                    player
                    LEFT JOIN enemplay ON enemplay.playerid = player.id
                    LEFT JOIN itemplay ON itemplay.playerid = player.id
                    INNER JOIN profile ON player.profileid = profile.id
                    INNER JOIN avatar ON player.avatarid = avatar.id
                    INNER JOIN avatardesc ON avatardesc.avatarid = avatar.id
                    INNER JOIN lang ON avatardesc.languageid = lang.id
                WHERE lang.id = $langid
                GROUP BY player.id;";
        break;
    default:
        break;
}

ReturnQuery($sql);