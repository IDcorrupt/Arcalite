<?php

require_once "config.php";

checkValidity("GET", "userid", "langid", "request_type");

$userid = $_GET['userid'];

switch($_GET['request_type']) {
    case "ENEMY":
        $langid = $_GET['langid'];

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
        $langid = $_GET['langid'];

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
        break;

    case "STATISTICS":
        $sql = "SELECT 
                (SELECT COUNT(*) FROM enemplay WHERE playerid = $userid) AS `enemyFound`,
                (SELECT COUNT(*) FROM enemy) AS `enemyAll`,
                (SELECT COUNT(*) FROM itemplay WHERE playerid = $userid) AS `itemFound`,
                (SELECT COUNT(*) FROM item) AS `itemAll`;";
        break;
    default:
        ReturnError(400, "Hiba az API-hívásban.");
}

ReturnQuery($sql);