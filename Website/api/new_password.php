<?php

require_once "config.php";

checkValidity("POST", "email", "password");

require_once "connection.php";

$email = $_POST['email'];
$password = $_POST['password'];


$sql = "UPDATE `profile` SET `password` = PASSWORD('$password') WHERE `email` = '$email';";
$result = $db->query($sql);


$return = array();
if ($result) {
    $return["code"] = 201;
    $return["message"] = "Új jelszó sikeresen beállítva!";
} else {
    $return["code"] = 500;
    $return["message"] = "Hiba a frissítés közben! Az új jelszó nem került beállításra!";
}

ReturnResult($return, $return['code']);