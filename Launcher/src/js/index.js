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

let settingsWin = null

const webIP = 'localhost'
const webPort = 3000

// We received a new download progress value
ipcRenderer.on('progress', (event, arg) => {
	progress = Math.round(arg * 100)
})

// Render the download progress bar
function renderProgressBar() {
	console.log(width)
	if (width >= 100) {
		clearInterval(renderInterval)
		downloading = false

		new Notification('Update Complete', {
			body: 'Client is now up-to-date.'
		})
		return
	}

	if (progress > width) {
		width = lerp(width, progress, progress / 100).toFixed(2)
		launchBarProgress.style.width = width + '%'
	}
}

/*document.addEventListener('mousemove', () => {
	if (settingsOpen) {
		settingsWin.focus()
	}
})*/

// Menu Buttons
close.addEventListener('click', () => {
	BrowserWindow.getFocusedWindow().close()
})

minimize.addEventListener('click', () => {
	BrowserWindow.getFocusedWindow().minimize()
})

settingsButton.addEventListener('click', () => {
	if (settingsOpen) {
		return
	}

	settingsOpen = true

	settingsWin = new BrowserWindow({
		width: 400,
		height: 300,
		title: 'Settings',
		show: false,
		frame: false,
		webPreferences: {
			nodeIntegration: true,
			enableRemoteModule: true
		}
	})

	settingsWin.on('ready-to-show', () => {
		settingsWin.moveTop()
		settingsWin.show()
	})

	settingsWin.on('closed', () => {
		settingsOpen = false
		settingsWin = null
	})

	settingsWin.loadFile('../src/settings.html')
})

// Launch Button
launchButton.addEventListener('click', () => {
	if (downloading) { // Only launch if not downloading anything
		return
	}

	// Tell the main process what to download
	/* TEST FILES 
	 * http://ipv4.download.thinkbroadband.com/5MB.zip
	 * http://ipv4.download.thinkbroadband.com/10MB.zip
	 * http://ipv4.download.thinkbroadband.com/20MB.zip
	 * http://ipv4.download.thinkbroadband.com/50MB.zip
	 */

	let platform = 'win'
	let release = 'latest'

	ipcRenderer.send('download-button', { url: `http://${webIP}:${webPort}/api/releases/${platform}/${release}.zip` })
	width = 0
	downloading = true
	renderInterval = setInterval(renderProgressBar, 10)
})

function lerp(v0, v1, t) {
	return (1 - t) * v0 + t * v1
}
