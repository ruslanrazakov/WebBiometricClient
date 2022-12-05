let _stream;

let videoLengthInMS = 800;

var dotNetGlobal = {};
dotNetGlobal.DotNetReference = null;
dotNetGlobal.SetDotnetReference = function (pDotNetReference) {
    dotNetGlobal.DotNetReference = pDotNetReference;
};


function startVideo() {
    var video = document.getElementById('video');

    if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
        navigator.mediaDevices.getUserMedia({ video: true }).then(function (stream) {
            _stream = stream;

            _stream.stop = function () {
                this.getAudioTracks().forEach(function (track) {
                    track.stop();
                });
                this.getVideoTracks().forEach(function (track) {
                    track.stop();
                });
            };

            try {
                video.srcObject = stream;
            } catch (error) {
                video.src = window.URL.createObjectURL(stream);
            }
            video.play();
        });
    }
}

function recordVideoAndSendToServer(url, engineId) {
    record().then(recordedChunks => {
        const headers = {
            engine_id: engineId,
            type: 'video/mp4'
        };
        let recordedBlob = new Blob(recordedChunks, headers);
        var formData = new FormData();
        formData.append('data', recordedBlob);
        var xhr = new XMLHttpRequest();

        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                console.log(this.responseText);
                dotNetGlobal.DotNetReference.invokeMethod('GetLivenessFromNtech', this.responseText);
            }
        };

        xhr.open('POST', url + "/" + engineId);
        xhr.setRequestHeader("engine-id", engineId);

        xhr.send(formData);
    });
}

const blobToBase64 = blob => {
    const reader = new FileReader();
    reader.readAsDataURL(blob);
    return new Promise(resolve => {
        reader.onloadend = () => {
            resolve(reader.result);
        };
    });
};

function record() {
    let recorder = new MediaRecorder(_stream);
    let data = [];

    recorder.ondataavailable = event => data.push(event.data);
    recorder.start();
    console.log(recorder.state + " for " + (videoLengthInMS / 1000) + " seconds...");

    let stopped = new Promise((resolve, reject) => {
        recorder.onstop = resolve;
        recorder.onerror = event => reject(event.name);
    });

    let recorded = wait(videoLengthInMS).then(
        () => recorder.state == "recording" && recorder.stop()
    );

    return Promise.all([
        stopped,
        recorded
    ]).then(() => data );
}

function wait(delayInMS) {
    return new Promise(resolve => setTimeout(resolve, delayInMS));
}

function stopVideo() {
    if (_stream !== "undefined")
        _stream.stop();
    else
        alert("Chrome заблокировал видео на данной странице. Попробуйте запустить браузер с аргументами start chrome --unsafely-treat-insecure-origin-as-secure=\"http://10.124.33.92:5003\"");
}

window.Snap = async (src, dest) => {
    let video = document.getElementById(src);
    let ctx = get2DContext(dest);
    ctx.drawImage(video, 0, 0, 480, 360);
}

window.GetImageData = async (el, format) => {
    let canvas = document.getElementById(el);
    let dataUrl = canvas.toDataURL(format);
    return dataUrl.split(',')[1];
}

function get2DContext(el) {
    return document.getElementById(el).getContext('2d');
}
