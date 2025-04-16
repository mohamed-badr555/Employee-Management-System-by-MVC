
var searchInp = document.getElementById("searchInp");

searchInp.addEventListener("keyup", function () {
    let xhr = new XMLHttpRequest();
    let url = `https://localhost:44356/Employee/Index?searchInp=${searchInp.value}`;
    xhr.open("post", url, true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            document.getElementById("tbody").innerHTML = xhr.responseText;
        }
    }
    xhr.send();
})