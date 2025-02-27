var desc = false; 
var lastOrdered = null;
var currentTable = null;

$(document).ready(function() {
    document.getElementById("defaultOpen").click();
    FetchRankings("Profile");
    FetchRankings("GameThrough");
});

function FetchRankings(type) {
    let payload = {
        type: type,
        langid: getCookie("langid")
    }

    $.ajax({
        type: "GET",
        url: "api/ranking.php",
        data: payload,
        global: false,
        success: (data) => {
            FillTable(type, data);
        },
        error: (data) => {
            alert(`Hiba az adatok lekérésében!`);
            console.error(`Hiba az API kérésben: ${data.code} ${data.message}`);
        }
    });
}

function FillTable(type, data) {
    let username = getCookie("username");
    switch(type) {
        case "Profile":
            for (let i=0; i < data.length; i++) {
                let format = (username == data[i].Felhasználónév) ? "userrow" : "";
                $(`#${type} tbody`).append(
                    `<tr class="${format}">
                        <td>${data[i].Felhasználónév}</td>
                        <td class="text-center">${data[i].Játékidő}</td>
                        <td class="text-center">${data[i].Elért_mérföldkövek}</td>
                        <td class="text-center">${data[i].Végigjátszások}</td>
                    </tr>`);
            }
            break;
        case "GameThrough":
            for (let i=0; i < data.length; i++) {
                let format = (username == data[i].Profil) ? "userrow" : "";
                $(`#${type} tbody`).append(
                    `<tr class="${format}">
                        <td>${data[i].Profil}</td>
                        <td>${data[i].Karakter}</td>
                        <td>${data[i].Avatár}</td>
                        <td class="text-center">${data[i].Játékidő}</td>
                        <td class="text-center">${data[i].Elért_szint}</td>
                        <td class="text-center">${data[i].Felfedezett_ellenfelek}</td>
                        <td class="text-center">${data[i].Felfedezett_tárgyak}</td>
                    </tr>`);
            }
            break;
    }
    
}

function sortTable(tablename, columnIndex) {
    if (tablename == currentTable && lastOrdered != null && columnIndex == lastOrdered) {
        desc = !desc;
    } else {
        desc = false;
    }

    lastOrdered = columnIndex;
    currentTable = tablename;

    let table = document.querySelector(`#${tablename} table`);
    let rows = Array.from(table.querySelectorAll("tr"));
    rows.shift();
    rows.sort(function(rowA, rowB) {
      var cellA = rowA.cells[columnIndex].textContent;
      var cellB = rowB.cells[columnIndex].textContent;
  
      // Handle numerical values
      if (!isNaN(cellA) && !isNaN(cellB)) {
        return desc ? cellB-cellA : cellA - cellB;
      }
  
      // Default to string comparison
      return cellA.localeCompare(cellB) * (desc ? -1 : 1);
    });
  
    rows.forEach(function(row) {
      table.querySelector("tbody").appendChild(row);
    });
}