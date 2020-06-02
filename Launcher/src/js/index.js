const { ipcRenderer } = require('electron')
const launchBarProgress = document.getElementById('launchBarProgress')
const launchButton = document.getElementById('launchButton')

let renderInterval

let downloading = false

let progress = 0
let width = 0

ipcRenderer.on('progress', (event, arg) => {
	progress = Math.round(arg * 100)
})

function renderProgressBar() {
	if (width >= 100) {
		clearInterval(renderInterval)
		downloading = false
		return
	}

	if (progress > width) {
		width++
		launchBarProgress.style.width = width + '%'
	}
}

launchButton.addEventListener('onclick', function() {
	if (downloading) {
		return
	}

	ipcRenderer.send('download-button', { url: 'https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png' })
	width = 0
	downloading = true
	renderInterval = setInterval(renderProgressBar, 10)
})