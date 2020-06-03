const { app, BrowserWindow, Menu, ipcMain } = require('electron')
const { download } = require('electron-dl')
const isDev = require('electron-is-dev')

let mainWin = null
let settingsWin = null

function createMainWindow() {
	mainWin = new BrowserWindow({
		width: 600,
		height: 400,
		title: 'Launcher',
		show: false,
		center: true,
		resizable: true,
		frame: false,
		webPreferences: {
			nodeIntegration: true
		}
	})

	mainWin.on('ready-to-show', () => {
		mainWin.show()
	})

	mainWin.loadFile('../src/index.html')
}

app.on('ready', () => {
	clearConsole()
	createMainWindow()
	Menu.setApplicationMenu(null)
	initDevTools()

	mainWin.on('closed', () => {
		app.quit()
	})

	console.log('Electron app is up and running..')
})

ipcMain.on('download-button', async (event, { url }) => {
	const win = BrowserWindow.getFocusedWindow()
	const options = {
		directory: app.getPath('pictures'),
		onProgress
	}
	await download(win, url, options)
})

function onProgress(obj) {
	mainWin.webContents.send('progress', obj.percent)
}

function initDevTools() {
	if (isDev) {
		const devMenuTemplate = [
			{
				label: 'Developer Tools',
				submenu: [
					{
						label: 'Toggle DevTools',
						accelerator: 'CmdOrCtrl+Shift+I',
						click(item, focusedWindow) {
							focusedWindow.toggleDevTools()
						}
					},
					{
						role: 'reload'
					}
				]
			}
		]
		const menu = Menu.buildFromTemplate(devMenuTemplate)
		Menu.setApplicationMenu(menu)
	}
}

function clearConsole() {
	const readline = require('readline')
	const blank = '\n'.repeat(process.stdout.rows)
	console.log(blank)
	readline.cursorTo(process.stdout, 0, 0)
	readline.clearScreenDown(process.stdout)
}