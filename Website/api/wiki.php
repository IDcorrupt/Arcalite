<?php

require_once "config.php";

checkValidity("GET", "userid", "langid", "request_type");

$userid = $_GET['userid'];
$langid = $_GET['langid'];

switch($_GET['request_type']) {
    case "ENEMY":
        $sql = "SELECT DISTINCT
                    enemy.id AS `id`,
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
        $sql = "SELECT DISTINCT
                    item.id AS `id`,
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
                (   SELECT COUNT(DISTINCT enemyid)
                    FROM enemplay
                        INNER JOIN player ON enemplay.playerid = player.id
                        INNER JOIN profile ON player.profileid = profile.id
                    WHERE profileid = $userid
                ) AS `enemyFound`,
                (SELECT COUNT(*) FROM enemy) AS `enemyAll`,
                (   SELECT COUNT(DISTINCT itemid)
                    FROM itemplay
                        INNER JOIN player ON itemplay.playerid = player.id
                        INNER JOIN profile ON player.profileid = profile.id
                    WHERE profileid = $userid
                ) AS `itemFound`,
                (SELECT COUNT(*) FROM item) AS `itemAll`;";
        break;
    default:
        ReturnMessage(400, "Hiba az API-hívásban.");
}

ReturnQuery($sql);