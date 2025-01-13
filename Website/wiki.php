<!DOCTYPE html>
<html lang="hu">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Document</title>
    <link rel="stylesheet" href="style.css">
     <!-- Bootstrap CSS -->
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet">

    <!-- jQuery and Bootstrap JS -->
    <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
</head>
<?php
    $servername = "localhost";
    $username = "root";
    $password = "";
    $dbname = "arcalite";

    // Create connection
    $conn = new mysqli($servername, $username, $password, $dbname);

    // Check connection
    if ($conn->connect_error) {
        die("Connection failed: " . $conn->connect_error);
    }
    echo "Connected successfully";
?>

<body>
    <nav class="navbar navbar-expand-sm navbar-light bg-light sticky-top">
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
            <ul class="navbar-nav mr-auto">
                <li class="nav-item"><a class="nav-link" href="./index.html">Kezdőlap</a></li>
                <li class="nav-item"><a class="nav-link disabled" href="#controlid">Irányítás</a></li>
                <li class="nav-item"><a class="nav-link border-right disabled" href="#characterid">Karakterek</a></li>
                <li class="nav-item active"><a class="nav-link border-left" href="./wiki.php">Lexikon</a></li>
                <li class="nav-item"><a class="nav-link" href="./ranking.html">Ranglista</a></li>
                <li class="nav-item ml-auto"><a class="nav-link" href="./profile.html">Bejelentkezés</a></li>
            </ul>
        </div>
    </nav>
    <header class="flex-container">
        <h1>Arcalite</h1>
    </header>
    
    <main>
        <h2>Tárgyak</h2>
        <div>
            <!-- pics, name and detalis from database
                default grey img and no text if havent obtained yet
             -->
        </div>
        <h2>Ellenségek</h2>
        <div>

        </div>
    </main>