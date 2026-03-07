const menuEl = document.getElementById("menu")
const newMapEl = document.getElementById("newMap")
const menu2El = document.getElementById("menu2")
const backEl = document.getElementById("back")
const gridEl = document.getElementById("grid")

let map = []

function createGrid(){

    gridEl.innerHTML = ""
    map = []

    for(let y = 0; y < 20; y++){

        let row = []

        for(let x = 0; x < 20; x++){

            const cell = document.createElement("div")
            cell.classList.add("cell")

            cell.dataset.x = x
            cell.dataset.y = y

            gridEl.appendChild(cell)

            row.push("grass")

        }

        map.push(row)

    }

}

gridEl.addEventListener("click", (e)=>{

    if(!e.target.classList.contains("cell")) return

    const x = e.target.dataset.x
    const y = e.target.dataset.y

    const type = map[y][x]

    if(type === "grass"){
        map[y][x] = "road"
        e.target.classList.add("road")
    }
    else if(type === "road"){
        map[y][x] = "water"
        e.target.classList.remove("road")
        e.target.classList.add("water")
    }
    else{
        map[y][x] = "grass"
        e.target.classList.remove("water")
    }

})

newMapEl.addEventListener("click", () => {

    menuEl.classList.add("hidden")
    menu2El.classList.remove("hidden")

    createGrid()

})

backEl.addEventListener("click", () => {

    menu2El.classList.add("hidden")
    menuEl.classList.remove("hidden")

})