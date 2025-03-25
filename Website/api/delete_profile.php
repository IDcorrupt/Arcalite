<?php

require_once "config.php";

checkValidity("POST", "userid");

require_once "connection.php";

$userid = $_POST["userid"];

$sql = "UPDATE profile SET deletedAT = CURRENT_TIMESTAMP() WHERE id = $userid";

$result = $db->query($sql);

if ($result) 
    ReturnMessage(200, "Profil sikeresen törölve!");
else
    ReturnMessage(500, "Hiba a törlés során! A profil nem került törlésre!");
