function CreateCourse() {
    var form = document.getElementById(`course`);
    var formData = new FormData(form);

    var skills = $("#Skills").serializeArray();
    skills.forEach(function (fields) {
        formData.append(`Skills${fields.name}`, fields.value);
    });

    var materials = $("#materials").serializeArray();
    materials.forEach(function (fields) {
        formData.append(`Materials${fields.name}`, fields.value);
    });

    $.ajax({
        type: "POST",
        url: `/Courses/Create`,
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.success) {
                window.location.href = "/Courses/Index";
            }
            else {
                $(`#createCourse`).html(data);
            }
        }
    });
}

function LoadAddSkills() {
    ChangePage(1, 'Skills')
}

function LoadAddVideos() {
    ChangeMaterialsPage(1, 'Videos')
}

function LoadAddArticles() {
    ChangeMaterialsPage(1, 'Articles')
}

function LoadAddBooks() {
    ChangeMaterialsPage(1, 'Books')
}

function ChangePage(pageNumber, url) {
    var list = $(`#${url}`).serialize();
    $.ajax({
        type: "GET",
        url: `/${url}/Index`,
        data: list + '&PageNumber=' + pageNumber,
        success: function (data) {
            $("#addPanel").html(data);
        }
    });
}

function ChangeMaterialsPage(pageNumber, url) {
    var list = $(`#materials`).serialize();
    $.ajax({
        type: "GET",
        url: `/${url}/Index`,
        data: list + '&PageNumber=' + pageNumber,
        success: function (data) {
            $("#addPanel").html(data);
        }
    });
}

function Create(url, pageNumber) {
    var model = $(`#create${url}`).serialize();
    $.ajax({
        type: "POST",
        url: `/${url}/Create`,
        data: model,
        success: function (data) {
            if (data.success) {
                ChangePage(pageNumber, url);
            }
            else {
                $(`#create${url}`).html(data);
            }
        }
    });
}

function Add(id, url) {
    var list = $(`#${url}`).serialize();
    $.ajax({
        type: "POST",
        url: `/${url}/Add`,
        data: list + `&id${url}=` + id,
        success: function (data) {
            $(`#${url}`).html(data);
            document.getElementById(`${url}${id}`).setAttribute("onClick", `Remove(${id}, '${url}');`);
        }
    });
}

function AddMaterial(id, url) {
    var list = $(`#materials`).serialize();
    $.ajax({
        type: "POST",
        url: `/${url}/Add`,
        data: list + `&id${url}=` + id,
        success: function (data) {
            $(`#materials`).html(data);
            document.getElementById(`${url}${id}`).setAttribute("onClick", `RemoveMaterial(${id}, '${url}');`);
        }
    });
}

function Remove(id, url) {
    var list = $(`#${url}`).serialize();
    $.ajax({
        type: "POST",
        url: `/${url}/Remove`,
        data: list + `&id${url}=` + id,
        success: function (data) {
            $(`#${url}`).html(data);
            document.getElementById(`${url}${id}`).setAttribute("onClick", `Add(${id}, '${url}');`);
        }
    });
}

function RemoveMaterial(id, url) {
    var list = $(`#materials`).serialize();
    $.ajax({
        type: "POST",
        url: `/${url}/Remove`,
        data: list + `&id${url}=` + id,
        success: function (data) {
            $(`#materials`).html(data);
            document.getElementById(`${url}${id}`).setAttribute("onClick", `AddMaterial(${id}, '${url}');`);
        }
    });
}

function Delete(id, url, pageNumber) {
    $.ajax({
        type: "POST",
        url: `/${url}/Delete/`,
        data: `&id${url}=` + id,
        success: function (data) {
            if (data.success) {
                ChangePage(pageNumber, url);
                Remove(id, url);
            } else {
                alert(data.message);
            }
        }
    });
}

function DeleteMaterial(id, url, pageNumber) {
    $.ajax({
        type: "POST",
        url: `/${url}/Delete/`,
        data: `&id${url}=` + id,
        success: function (data) {
            if (data.success) {
                ChangeMaterialsPage(pageNumber, url);
                RemoveMaterial(id, url);
            } else {
                alert(data.message);
            }
        }
    });
}

function CreateMaterial(url, pageNumber) {
    var form = document.getElementById(`form${url}`);
    var formData = new FormData(form);
    var files = $('#File').prop("files");
    formData.append("File", files[0]);

    $.ajax({
        type: "POST",
        url: `/${url}/Create`,
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            if (data.success) {
                ChangeMaterialsPage(pageNumber, `${url}`);
            }
            else {
                $(`#create${url}`).html(data);
            }
        }
    });
}

const getVideoDuration = (file) =>
    new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => {
            const media = new Audio(reader.result);
            media.onloadedmetadata = () => resolve(media.duration);
        };
        reader.readAsDataURL(file);
        reader.onerror = (error) => reject(error);
    });

const handleChange = async (e) => {
    const duration = await getVideoDuration(e.target.files[0]);
    document.getElementById("Duration").value = parseInt(duration);
};

function FileToLink() {
    var formData = new FormData();
    var files = $('#thumbnailFile').prop("files");
    formData.append("file", files[0]);

    $.ajax({
        type: "POST",
        url: `/Courses/GetThumbnail`,
        data: formData,
        contentType: false,
        processData: false,
        success: function (data) {
            $(`#thumbnailImage`).html(data);
        }
    });
}