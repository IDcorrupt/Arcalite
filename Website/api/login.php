<?php

require_once "config.php";

if ($_SERVER["REQUEST_METHOD"] != "GET") {
    ReturnError(405, "HIBA! Hiba az API-hívásban.");
}

if (!( isset($_GET["email"], $_GET["password"]) && count($_GET) == 2) ) {
    ReturnError(400, "HIBA! Hiba az API-hívásban.");
}

require_once "connection.php";

$email = $_GET['email'];
$password = $_GET['password'];

$query = "SELECT COUNT(*) AS `db` FROM `profile` WHERE `email` = '$email';";
$result = $db->query($query);
$num_of_profiles = intval($result->fetch_assoc()['db']);

if ($num_of_profiles == 0) {
    ReturnError(401,"HIBA! Nincs ehhez az e-mail címhez regisztrált fiók!");
}

$query = "SELECT COUNT(*) AS `db` FROM `profile` WHERE `email` = '$email' AND `password` = PASSWORD('$password');";
$result = $db->query($query);
$num_of_profiles = intval($result->fetch_assoc()['db']);

if ($num_of_profiles == 0) {
    ReturnError(401,"HIBA! Hibás jelszó!");
}
else if ($num_of_profiles > 1) {
    ReturnError(401, "HIBA! Több fiók bejelentkezési adatai egyeznek!");
}

$query = "SELECT * FROM `profile` WHERE `email` = '$email' AND `password` = PASSWORD('$password');";
$result = $db->query($query);
$id = $result->fetch_assoc();

echo json_encode($id, JSON_PRETTY_PRINT);
http_response_code(200);
