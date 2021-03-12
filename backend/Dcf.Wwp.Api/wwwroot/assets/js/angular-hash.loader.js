(function () {
    // Check if the DOMContentLoaded has already been completed
    if (document.readyState === 'complete' || document.readyState !== 'loading') {
        onDocumentLoaded();
    } else {
        document.addEventListener('DOMContentLoaded', function () {
            onDocumentLoaded();
        }, false);
    }

    var onDocumentLoaded = function () {
        var scripts = document.scripts;
        for (var i = 0; i < scripts.length; i++) {
            var script = scripts[i];
            var src = script.src;
            var leafname = src.split('\\').pop().split('/').pop();
            if (leafname && leafname.startsWith('main.') && leafname.endsWith('.bundle.js')) {
                var mainHash = leafname.substring(6, leafname.length - 10);
                localStorage.setItem("clientMainHash", mainHash);
            }
        }
    }

}());
