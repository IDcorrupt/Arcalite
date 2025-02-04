<?php

require_once "config.php";

if ($_SERVER["REQUEST_METHOD"] != "POST") {
    ReturnError(405, "Hiba az API-hívásban.");
}

checkProperFields("POST", "email", "password");


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

header("Content-Type: application/json");
echo json_encode($return);
http_response_code($return["code"]);