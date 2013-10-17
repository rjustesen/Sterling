// Place common javascript here

// Clear all text input boxes on in the document
function clearAllText() {
        
    var txt = document.getElementsByTagName('input');
    for (var i = 0; i < txt.length; i++) {
        if (txt[i].type === 'text')
            txt[i].value = "";
    }
}
// Toggle all check boxes in the document
function toggleCheckBoxes(checked) {
    var chk = document.getElementsByTagName('input');
    var len = chk.length;
    for (var i = 0; i < len; i++) {
        if (chk[i].type === 'checkbox')
            chk[i].checked = checked;
    }
}
// Close a window
function window_close() {
    window.opener = top;
    window.close();
}
// ToolTip functions - Relies on jquery!
var tooltip = function() {
    var id = 'tt';
    var top = 3;
    var left = 3;
    var maxw = 300;
    var speed = 10;
    var timer = 20;
    var endalpha = 95;
    var alpha = 0;
    var tt, t, c, b, h;
    var ie = document.all ? true : false;
    return {
        show: function(v, w) {
            if (tt == null) {
                tt = document.createElement('div');
                tt.setAttribute('id', id);
                t = document.createElement('div');
                t.setAttribute('id', id + 'top');
                c = document.createElement('div');
                c.setAttribute('id', id + 'cont');
                b = document.createElement('div');
                b.setAttribute('id', id + 'bot');
                tt.appendChild(t);
                tt.appendChild(c);
                tt.appendChild(b);
                document.body.appendChild(tt);
                tt.style.opacity = 0;
                tt.style.filter = 'alpha(opacity=0)';
                document.onmousemove = this.pos;
            }
            tt.style.display = 'block';
            c.innerHTML = v;
            tt.style.width = w ? w + 'px' : 'auto';
            if (!w && ie) {
                t.style.display = 'none';
                b.style.display = 'none';
                tt.style.width = tt.offsetWidth;
                t.style.display = 'block';
                b.style.display = 'block';
            }
            if (tt.offsetWidth > maxw) { tt.style.width = maxw + 'px' }
            h = parseInt(tt.offsetHeight) + top;
            clearInterval(tt.timer);
            tt.timer = setInterval(function() { tooltip.fade(1) }, timer);
        },
        pos: function(e) {
            var u = ie ? event.clientY + document.documentElement.scrollTop : e.pageY;
            var l = ie ? event.clientX + document.documentElement.scrollLeft : e.pageX;
            tt.style.top = (u - h) + 'px';
            tt.style.left = (l + left) + 'px';
        },
        fade: function(d) {
            var a = alpha;
            if ((a != endalpha && d == 1) || (a != 0 && d == -1)) {
                var i = speed;
                if (endalpha - a < speed && d == 1) {
                    i = endalpha - a;
                } else if (alpha < speed && d == -1) {
                    i = a;
                }
                alpha = a + (i * d);
                tt.style.opacity = alpha * .01;
                tt.style.filter = 'alpha(opacity=' + alpha + ')';
            } else {
                clearInterval(tt.timer);
                if (d == -1) { tt.style.display = 'none' }
            }
        },
        hide: function() {
            clearInterval(tt.timer);
            tt.timer = setInterval(function() { tooltip.fade(-1) }, timer);
        }
    };
} ();

function expandDiv(obj)
{
    var div = document.getElementById(obj);
    var img = document.getElementById('imgdiv');
    if (div.style.display == "none") {
        div.style.display = "inline";
        img.src = "~/assets/images/close.gif";
        img.alt = "Click to Collapse";
    } else {
        div.style.display = "none";
        img.src = "~/assets/images/detail.gif";
        img.alt = "Click to Expand";
    }
}
