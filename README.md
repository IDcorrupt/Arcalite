# Arcalite

## Tartalomjegyzék

1. [A projekt bemutatása](#A-projekt-bemutatása)
    1. [A játék](#A-játék)
    1. [A weboldal](#A-weboldal)
1. [A repository fájlrendszere](#A-repository-fájlrendszere)
1. [A webszerver felállítása a XAMPP segítségével](#A-webszerver-felállítása-a-XAMPP-segítségével)

## A projekt bemutatása

Projektünk két fő részből áll: egy játék és egy hozzá kapcsolódó weboldal.

### A játék

A játék egy kétdimenziós, oldalnézetes platformer. Főhösünk egy mágus, aki pályákon átkelve, szörnyeket legyőzve próbál eljutni céljához. Hogy mi a célja? A játék végén kiderül...

#### Főbb irányítások

- `A/D` - mozgás balra/jobbra
- `W` - ugrás
- `S` - guggolás
- `Shift` - dash (előreugrás)
- `Bal egérgomb` - lövés
- `Jobb egérgomb` - töltőlövés (minél tovább tartjuk lent, annál nagyobbat lő)
- `Szóköz` - oracle (lassító varázskör)
- ˙E` - 1. varázslat
- `Q` - 2. varázslat

### A weboldal

A weboldal három célt szolgál: egy bemutató és tudástár a játékhoz, néhány statisztika megjelenítése és profilkezelés.

Az oldalra alapból vendégként lépünk fel, viszont a menüben lehetőségünk van bejelentkezni. A bejelentkezés oldalon tudunk regisztrálni is. Bejelentkezés után megjelenik a *Profil* menüpont, melybe belépve láthatjuk külön játékmeneteink adatait, illetve a profilműveleteket végző gombokat. Itt lehetőségünk van nevet, e-mailt, jelszót változtatni, kijelentkezni és törölni a profilt. Az ezekhez szükséges kommunikáció, bemenet stb. az oldal tetején felugróablakokkal történik.

A tudástár vendégként nem elérhető. Bejelentkezés után válik elérhetővé, viszont akkor is csak arról olvashatunk el információkat, amikkel már találkoztunk.

A statisztika oldal elérhető vendégként is, viszont bejelentkezés után kiemelve látjuk a hozzánk tartozó adatokat.

## A repository fájlrendszere

- A játék forráskódja az **ArcaliteGame** mappában található. Ez egy GODOT-project mappája, a futtatható állomány az **Exports\Exe** mappában található.
- A weboldal a **Website** mappában található. A helyes működéshez ez kell legyen a webszerver gyökere.
- A dokumentáció Word-dokumentuma és a prezentációnk a **Dok** mappában található.

## A webszerver felállítása a XAMPP segítségével

A fejlesztés során a webszerver felállítását a XAMPP programcsomag segítségével oldottuk meg. Ennek folyamata a következő:

1. Megnyitjuk a **XAMPP kezelőfelület**et, és elindítjuk az **Apache**-szervert és a **MySQL** szolgáltatást.
2. Megnyitjuk a **parancssor**t, majd elnavigálunk a projekt **Website** mappájába.
3. Futtatjuk a következő parancsot: **`php -S localhost:1234`**, ahol az *1234* helyére egy tetszőleges (szabad) portszámot megadhatunk.
    - Abban az esetben, ha a parancssor nem ismeri fel a `php` parancsot, két lehetőségünk van:
        1. A `php` helyett a XAMPP mappáján belüli php mappa elérési útjával együtt futtatjuk a parancsot (alapesetben `C:\xampp\php`, tehát a parancs `C:\xampp\php\php -S localhost:1234`)
        2. A **PATH** környezeti változóhoz hozzáadjuk az előbb leírt php mappát (alapesetben `C:\xampp\php`)
4. Megnyitunk egy böngészőt, és felkeressük a **`localhost:80/phpmyadmin/`** címet (ahol 80 a XAMPP kezelőfelületen az `Apache` mellett található szám, amely nem a 443)
5. Az **Importálás** menüpontot választjuk, majd a fájl kiválasztásánál kiválasztjuk a **Website\Database** mappából az **`arcalite.sql`** fájlt (ez tartalmaz tesztadatokat, ha ezt nem szeretnénk válasszuk a `notestdata.sql` fájlt).
6. Az oldal alján az **Importálás** gombbal betöltjük az adatbázist.
7. Ezután a weboldalt a **`localhost:1234`** címen érjük el (ahol 1234 az előzőekben megadott port)
