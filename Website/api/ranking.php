<?php

require_once "config.php";


if ($_SERVER["REQUEST_METHOD"] != "GET") {
    ReturnError(405, "Hiba az API-hívásban.");
}

checkProperFields("GET", "type");

switch ($_GET["type"]) {
    case "profile":
        $sql = "SELECT
                    profile.username AS `Felhasználónév`,
                    profile.played AS `Játékidő`,
                    COUNT(proach.achievementid) AS `Elért mérföldkövek`,
                    COUNT(CASE WHEN player.levelid = (SELECT MAX(id) FROM level) THEN player.id END) AS `Végigjátszások`
                FROM 
                    profile 
                    LEFT JOIN proach ON profile.id = proach.profileid
                    LEFT JOIN player ON profile.id = player.profileid
                GROUP BY profile.id;";
        break;
    case "game":
        //[lang]-nál valahogy az oldal vagy az ember nyelvét meg kell adni :3
        $sql = "SELECT
                    player.name AS `Karakter`,
                    profile.username AS `Profil`,
                    avatardesc.name AS `Ąvatár`,
                    player.playtime AS `Játékidő`,
                    player.levelid AS `Elért szint`,
                    COUNT(enemplay.enemyid) AS `Felfedezett ellenfelek`,
                    COUNT(itemplay.itemid) AS `Felfedezett tárgyak`
                FROM 
                    player
                    INNER JOIN profile ON player.profileid = profile.id
                    INNER JOIN avatar ON player.avatarid = avatar.id
                    INNER JOIN avatardesc ON avatardesc.avatarid = avatar.id
                    INNER JOIN lang ON avatardesc.languageid = lang.id
                    INNER JOIN enemplay ON enemplay.playerid = player.id
                    INNER JOIN itemplay ON itemplay.playerid = player.id
                WHERE lang = [lang];";
        break;
    default:
        break;
}

require_once "connection.php";

$result = $db->query($sql);

header("Content-Type: application/json");
echo json_encode($result, JSON_PRETTY_PRINT);
http_response_code(200);
