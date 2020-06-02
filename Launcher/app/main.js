const { app, BrowserWindow, Menu, ipcMain } = require('electron')
const { download } = require('electron-dl')
const isDev = require('electron-is-dev')

let mainWin = null

function createWindow() {
	mainWin = new BrowserWindow({
		width: 600,
		height: 400,
		center: true,
		resizable: true,
		webPreferences: {
			nodeIntegration: true
		}
	})

	mainWin.loadFile('../src/index.html')
}

app.on('ready', () => {
	clearConsole()
	createWindow()
	Menu.setApplicationMenu(null)
	initDevTools()

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