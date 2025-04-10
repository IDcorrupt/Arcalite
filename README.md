# Arcalite

## Tartalomjegyzék

1. [A projekt bemutatása](#A-projekt-bemutatása)
2. [A fájlrendszer](#A-fájlrendszer)
3. [A webszerver felállítása a XAMPP segítségével](#A-webszerver-felállítása-a-XAMPP-segítségével)

## A projekt bemutatása

Bemutatás

## A fájlrendszer

- A játék forráskódja az **ArcaliteGame** mappában található. Ez egy GODOT-project mappája, a futtatható állomány az **Exports\Exe** mappában található.
- A weboldal a **Website** mappában található. A helyes működéshez ez kell legyen a webszerver gyökere.
- A dokumentáció Word-dokumentuma és a prezentációnk a **Dok** mappában található.
    - A dokumentáció legfrissebb verziója a *docs* ágon található. A véglegesítés után a *main*-re kerül, ekkor ez a sor törlésre fog kerülni.

## A webszerver felállítása a XAMPP segítségével

A fejlesztés során a webszerver felállítását a XAMPP programcsomag segítségével oldottuk meg. Ennek folyamata a következő:

1. Megnyitjuk a **XAMPP kezelőfelület**et, és elindítjuk az **Apache**-szervert és a **MySQL** szolgáltatást.
2. Megnyitjuk a **parancssor**t, majd elnavigálunk a projekt **Website** mappájába.
3. Futtatjuk a következő parancsot: **`php -S localhost:1234`**, ahol az *1234* helyére egy tetszőleges (szabad) portszámot megadhatunk.
    - Abban az esetben, ha a parancssor nem ismeri fel a `php` parancsot, két lehetőségünk van:
        1. A `php` helyett a XAMPP mappáján belüli php mappa elérési útjával együtt futtatjuk a parancsot (alapesetben `C:\xampp\php`, tehát a parancs `C:\xampp\php\php -S localhost:1234`)
        2. A **PATH** környezeti változóhoz hozzáadjuk az előbb leírt php mappát (alapesetben `C:\xampp\php`)
4. Megnyitunk egy böngészőt, és felkeressük a `localhost:80/phpmyadmin/` címet (ahol 80 a XAMPP kezelőfelületen az `Apache` mellett található szám, amely nem a 443)
5. Az **Importálás** menüpontot választjuk, majd a fájl kiválasztásánál kiválasztjuk a **Website\Database** mappából az **`arcalite.sql`** fájlt (ez tartalmaz tesztadatokat, ha ezt nem szeretnénk válasszuk a `notestdata.sql` fájlt).
6. Az oldal alján az **Importálás** gombbal betöltjük az adatbázist.
7. Ezután a weboldalt a **`localhost:1234`** címen érjük el (ahol 1234 az előzőekben megadott port)
