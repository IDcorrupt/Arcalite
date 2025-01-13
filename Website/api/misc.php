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
    case "POST":
        switch($_POST['request_type'] ?? null) {
            case "UPDATE_PWD":
                update_pwd();
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

function update_pwd() {
    if (!isset($_POST['email'], $_GET['password']) && count($_POST) != 3) {
        ReturnError(400, "Hiba az API-hívásban.");
    }

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
}