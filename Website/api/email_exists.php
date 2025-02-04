<?php

require_once "config.php";

if ($_SERVER["REQUEST_METHOD"] != "GET") {
    ReturnError(405, "Hiba az API-hívásban.");
}

checkProperFields("GET", "email");


require_once "connection.php";

$email = $_GET['email'];

$sql = "SELECT * FROM `profile` WHERE email = '$email';";
$result = $db->query($sql);


$return = array(
    "exists" => $result->num_rows > 0
);

header("Content-Type: application/json");
echo json_encode($return);
http_response_code(200);