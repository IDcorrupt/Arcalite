<?php

require_once "config.php";

switch ($_SERVER["REQUEST_METHOD"]) {
    case "GET":
        switch($_GET['request_type'] ?? null) {
            case "EMAIL_EXISTS":
                email_exists();
                break;
            default:
                ReturnError(400, "Hiba az API-hívásban.");
        }
        break;
    default:
        ReturnError(405, "Hiba az API-hívásban.");
}

function email_exists() {
    if (!isset($_GET['email']) && count($_GET) != 2) {
        ReturnError(400, "Hiba az API-hívásban.");
    }

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
}