document.addEventListener("DOMContentLoaded", function() {

  // *** CANLI SAAT FONKSİYONU ***
  function updateClock() {
    const now = new Date();
    let hh = now.getHours().toString().padStart(2, '0');
    let mm = now.getMinutes().toString().padStart(2, '0');
    let ss = now.getSeconds().toString().padStart(2, '0');
    const clockEl = document.getElementById('liveClock');
    if (clockEl) {
      clockEl.textContent = `${hh}:${mm}:${ss}`;
    }
  }
  setInterval(updateClock, 1000);
  updateClock();

  // *** 'H' TUŞUNA BASILINCA FORM GİZLEME (Sadece index.html'de geçerli) ***
  document.addEventListener('keydown', function(e) {
    if (e.key === 'h' || e.key === 'H') {
      const toggle = document.getElementById('toggle');
      const container = document.querySelector('.container');
      // Eğer index.html'deki elemanlar varsa:
      if (toggle && container) {
        // Checkbox'ı kapat
        toggle.checked = false;
        // .container'a .hidden sınıfını toggle et
        container.classList.toggle('hidden');
      }
    }
  });

  // *** LOGIN SAYFASI KONTROLÜ ***
  const loginBtn = document.getElementById("loginBtn");
  if (loginBtn) {
    loginBtn.addEventListener("click", function() {
      const usernameValue = document.getElementById("usernameInput").value;
      const passwordValue = document.getElementById("passwordInput").value;
      
      // Varsayılan giriş bilgileri: admin / admin
      if (usernameValue === "admin" && passwordValue === "admin") {
        window.location.href = "table.html";
      } else {
        alert("Hatalı kullanıcı adı veya şifre. Tekrar deneyin!");
      }
    });
  }

  // *** TABLE SAYFASI KONTROLÜ ***
  const classForm = document.getElementById("classForm");
  const classTableBody = document.querySelector("#classTable tbody");

  // Eğer table.html'de isek ve ilgili elemanlar mevcutsa:
  if (classForm && classTableBody) {
    // Sınıf bilgilerini tutacak dizi
    let classesList = [];

    // *** REAL-TIME VALIDATION: Number of People sadece rakam olsun
    const numberInput = document.getElementById("classCount");
    numberInput.addEventListener("input", function() {
      // Rakam dışında bir karakter girildiyse sil
      this.value = this.value.replace(/[^0-9]/g, "");
    });

    // Form Submit Event => tabloya yeni satır ekle
    classForm.addEventListener("submit", function(e) {
      e.preventDefault();

      const className = document.getElementById("className").value.trim();
      const classCount = document.getElementById("classCount").value.trim();
      const classDesc = document.getElementById("classDescription").value.trim();

      // Boş bırakılamaz
      if (!className || !classCount || !classDesc) {
        alert("Lütfen tüm alanları doldurun!");
        return;
      }

      // Yeni satır oluştur
      const newRow = document.createElement("tr");
      newRow.dataset.clicked = "false"; // Henüz tıklanmadı (mouse-over highlight için)

      const nameCell = document.createElement("td");
      nameCell.textContent = className;

      const countCell = document.createElement("td");
      countCell.textContent = classCount;

      const descCell = document.createElement("td");
      descCell.textContent = classDesc;

      newRow.appendChild(nameCell);
      newRow.appendChild(countCell);
      newRow.appendChild(descCell);

      // Tabloya ekle
      classTableBody.appendChild(newRow);

      // Diziye ekle (daha sonra tabloyu loglayabilmek için)
      classesList.push({
        name: className,
        count: classCount,
        desc: classDesc
      });

      // Formu sıfırla
      classForm.reset();
    });

    // *** MOUSEOVER / MOUSEOUT => satır geçici highlight
    classTableBody.addEventListener("mouseover", function(e) {
      if (e.target && e.target.nodeName === "TD") {
        const row = e.target.parentNode;
        // Sadece tıklanmadıysa (clicked=false) geçici highlight uygula
        if (row.dataset.clicked === "false") {
          row.style.backgroundColor = "rgba(255, 255, 255, 0.1)";
        }
      }
    });
    classTableBody.addEventListener("mouseout", function(e) {
      if (e.target && e.target.nodeName === "TD") {
        const row = e.target.parentNode;
        // Sadece tıklanmadıysa rengi geri al
        if (row.dataset.clicked === "false") {
          row.style.backgroundColor = "";
        }
      }
    });

    // *** CLICK => satırın rengini kalıcı yap / kaldır + console.log
    classTableBody.addEventListener("click", function(e) {
      if (e.target && e.target.nodeName === "TD") {
        const row = e.target.parentNode;
        // Toggle mantığı
        if (row.dataset.clicked === "false") {
          row.dataset.clicked = "true";
          row.style.backgroundColor = "rgba(255, 0, 0, 0.2)";
          console.log("Clicked row data:", row.innerText);
        } else {
          row.dataset.clicked = "false";
          // Mouse üstünde ise mouseover rengi kalabilir; basitçe sıfırla:
          row.style.backgroundColor = "";
        }
      }
    });

    // *** DOUBLE-CLICK => satırı sil
    classTableBody.addEventListener("dblclick", function(e) {
      if (e.target && e.target.nodeName === "TD") {
        e.target.parentNode.remove();
      }
    });

    // *** TABLOYA (TABLE, THEAD, TBODY) TIKLANINCA TÜM SINIFLARI LOG
    const classTable = document.getElementById("classTable");
    classTable.addEventListener("click", function(e) {
      // Boş alana, thead'e veya tbody'ye tıklanırsa => Tüm entries'i logla
      // Not: TD'ye tıklanırsa yukarıdaki event tetikleniyor. Burada TABLE, THEAD, TBODY kontrol edebiliriz.
      if (
        e.target.nodeName === "TABLE" ||
        e.target.nodeName === "THEAD" ||
        e.target.nodeName === "TBODY"
      ) {
        console.log("All classes:", classesList);
      }
    });
  }

});
