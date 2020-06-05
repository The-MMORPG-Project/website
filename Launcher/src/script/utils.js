function id(id) {
    return document.getElementById(id)
}

function round(n, d) {
    return Math.round(n * (10 ** d)) / (10 ** d)
}

function lerp(start, end, amt) {
	return (1 - amt) * start + amt * end
}