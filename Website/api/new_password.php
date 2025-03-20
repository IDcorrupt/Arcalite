<?php

require_once "config.php";

checkValidity("POST", "email", "password");

require_once "connection.php";

$email = $_POST['email'];
$password = $_POST['password'];


$sql = "UPDATE `profile` SET `password` = PASSWORD('$password') WHERE `email` = '$email' AND `deletedAt` IS NULL;";
$result = $db->query($sql);

if ($result) {
    ReturnMessage(200, "Új jelszó sikeresen beállítva!");
}

ReturnMessage(500, "Hiba a frissítés közben! Az új jelszó nem került beállításra!");