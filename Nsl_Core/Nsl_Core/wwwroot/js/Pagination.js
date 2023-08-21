//分頁相關
let paginationIndex = 0 //不能調整
let arrLenght = 0 //必要!!!要給予原本拿到的資料的陣列長度
let range = 10 //必要!!!依照需求直接在頁面指定，不然就是預設顯示10筆資料
let pagination = []
let Update = function () {
    Load()
}
//把完整版陣列丟進去，會生成相對應頁數的控制器
//每次按按鈕都會執行一次Load
function PageItem(arr) {
    let re = ""
    for (let i = 0; i < Math.ceil(arr.length / range); i++) {
        if (i == ((paginationIndex + range) / range) - 1) {
            re += `<li class="page-item pageitems active" onclick="PageTo(${i + 1})"><a class="page-link" href="#">${i + 1}</a></li>`
        }
        else {
            re += `<li class="page-item pageitems" onclick="PageTo(${i + 1})"><a class="page-link" href="#">${i + 1}</a></li>`
        }
    }
    $("ul .pageitems").remove()
    $("ul .pages").after(re)
}
//去指定頁
function PageTo(page) {
    paginationIndex = (page * range) - range
    Update()
}
//上一頁
function PagePrevious() {
    paginationIndex = paginationIndex - range
    if ((paginationIndex - range) < 0) {
        paginationIndex = 0
    }
    Update()
}
//下一頁
function PageNext() {
    console.log(paginationIndex + range)
    console.log(arrLenght - 1)
    if ((paginationIndex + range) < (arrLenght - 1)) {
        paginationIndex = paginationIndex + range
    }
    Update()
}
//擷取陣列內容，把完整版的陣列丟進去
function Pagination(list) {
    const re = list.slice(paginationIndex, paginationIndex + range)
    return re
}