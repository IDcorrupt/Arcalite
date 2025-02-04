<?php

if ($_SERVER["REQUEST_METHOD"] != "GET") {
    ReturnError(405, "Hiba az API-hívásban.");
}

require_once "config.php";

checkProperFields("GET", "userid", "langid", "request_type");

require_once "connection.php";

$userid = $_GET['userid'];
$langid = $_GET['langid'];

switch($_GET['request_type']) {
    case "ENEMY":
        //ismert enemy-k lekérése
        $sql = "SELECT
                    enemy.hp AS `hp`,
                    enemy.image AS `image`,
                    enemydesc.name AS `name`,
                    enemydesc.description AS `desc`
                FROM
                    enemy
                    INNER JOIN enemplay ON enemy.id = enemplay.enemyid
                    INNER JOIN enemydesc ON enemy.id = enemydesc.enemyid
                WHERE 
                    enemydesc.languageid = $langid
                    AND
                    enemplay.playerid IN (SELECT id FROM player WHERE profileid = $userid)";
        break;
    case "ITEM":
        $sql = "SELECT
                    item.image AS `image`,
                    itemdesc.name AS `name`,
                    itemdesc.description AS `desc`
                FROM
                    item
                    INNER JOIN itemplay ON itemplay.itemid = item.id
                    INNER JOIN itemdesc ON itemdesc.itemid = item.id
                WHERE
                    itemdesc.languageid = $langid
                    AND
                    itemplay.playerid IN (SELECT id FROM player WHERE profileid = $userid) ;";
        //ismert tárgyak lekérése
        break;
    case "STATISTICS":
        //[ismert/összes] tárgyak és ellenfelek száma
        break;
    default:
        ReturnError(400, "Hiba az API-hívásban.");
}