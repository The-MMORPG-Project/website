const { ipcRenderer } = require('electron')
const { BrowserWindow } = require('electron').remote
const launchBarProgress = document.getElementById('launchBarProgress')
const launchButton = document.getElementById('launchButton')
const settingsButton = document.getElementById('settingsButton')
const close = document.getElementById('close')
const minimize = document.getElementById('minimize')

let renderInterval

let downloading = false
let settingsOpen = false

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

close.addEventListener('click', () => {
	BrowserWindow.getFocusedWindow().close()
})

minimize.addEventListener('click', () => {
	BrowserWindow.getFocusedWindow().minimize()
})

launchButton.addEventListener('click', () => {
	if (downloading) {
		return
	}

	ipcRenderer.send('download-button', { url: 'https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png' })
	width = 0
	downloading = true
	renderInterval = setInterval(renderProgressBar, 10)
})

settingsButton.addEventListener('click', () => {
	if (settingsOpen) {
		BrowserWindow.getFocusedWindow().close()
		return
	}

	settingsOpen = true

	let win = new BrowserWindow({
		width: 300,
		height: 200,
		title: 'Settings',
		show: false,
		alwaysOnTop: true,
		frame: false,
		webPreferences: {
			nodeIntegration: true,
			enableRemoteModule: true
		}
	})

	win.on('ready-to-show', () => {
		win.show()
	})

	win.on('close', () => {
		win = null
		settingsOpen = false
	})

	win.loadFile('../src/settings.html')
})
