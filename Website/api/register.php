<?php

require_once "config.php";

if ($_SERVER["REQUEST_METHOD"] != "POST") {
    ReturnError(405, "Hiba az API-hívásban.");
}

checkProperFields("POST", "username", "email", "password");


require_once "connection.php";

$username = $_POST['username'];
$password = $_POST['password'];
$email = $_POST['email'];


$query = "SELECT COUNT(*) AS `db` FROM `profile` WHERE `email` = '$email'";
$result = $db->query($query);
$num_of_users = intval($result->fetch_assoc()['db']);

if ($num_of_users > 0) {
    ReturnError(409, "Már van fiók regisztrálva ehhez az e-mail címhez!");
}


$query = "SELECT COUNT(*) AS `db` FROM `profile` WHERE `username` = '$username'";
$result = $db->query(query: $query);
$num_of_users = intval($result->fetch_assoc()['db']);

if ($num_of_users > 0) {
    ReturnError(409, "Már van fiók ilyen felhasználónévvel!");
}


$query = "INSERT INTO `profile` (`username`, `email`, `password`) VALUES ('$username','$email',PASSWORD('$password'))";
$result = $db->query($query);

if (!$result) {
    ReturnError(500, "Hiba az adatok feltöltése közben.");
}


$response = array(
    "code" => 201,
    "message" => "Sikeres regisztráció!"
);

ReturnResult($response, 201);