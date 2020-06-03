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

// We received a new download progress value
ipcRenderer.on('progress', (event, arg) => {
	progress = Math.round(arg * 100)
})

function lerp (start, end, amt){
  return (1-amt)*start+amt*end;
}

// Render the download progress bar
function renderProgressBar() {
	if (width >= 100) {
		clearInterval(renderInterval)
		downloading = false

		new Notification('Update Complete', {
			body: 'Client is now up-to-date.'
		})
		return
	}

	if (progress > width) {
		width = lerp(width, progress, 0.01)
		launchBarProgress.style.width = width + '%'
		console.log(progress);
	}

	document.querySelector("#launchButton").innerHTML = progress + "%"

}

// Menu Buttons
close.addEventListener('click', () => {
	BrowserWindow.getFocusedWindow().close()
})

minimize.addEventListener('click', () => {
	BrowserWindow.getFocusedWindow().minimize()
})

settingsButton.addEventListener('click', () => {
	if (settingsOpen) {
		BrowserWindow.getFocusedWindow().close()
		return
	}

	settingsOpen = true

	let win = new BrowserWindow({
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

	win.on('ready-to-show', () => {
		win.moveTop()
		win.show()
	})

	win.on('close', () => {
		win = null
		settingsOpen = false
	})

	win.loadFile('../src/settings.html')
})

// Launch Button
launchButton.addEventListener('click', () => {
	if (downloading) { // Only launch if not downloading anything
		return
	}
	console.log("Beginning Download");
	// Tell the main process what to download
	ipcRenderer.send('download-button', { url: 'https://icatcare.org/app/uploads/2018/07/Thinking-of-getting-a-cat.png' })
	width = 0
	downloading = true
	renderInterval = setInterval(renderProgressBar, 10)
	renderProgressBar()
})
