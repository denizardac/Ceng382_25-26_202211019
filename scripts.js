// Kullanıcı verilerini tutacak dizi
const users = [];

// Login butonuna tıklanınca kullanıcı verisini kaydet
document.getElementById("loginBtn").addEventListener("click", function() {
  const usernameValue = document.getElementById("usernameInput").value;
  const passwordValue = document.getElementById("passwordInput").value;

  // Diziye ekle
  users.push({
    username: usernameValue,
    password: passwordValue
  });

  // Konsolda göster
  console.log("All user data:", users);
});

// Canlı saat fonksiyonu
function updateClock() {
  const now = new Date();
  let hh = now.getHours().toString().padStart(2, '0');
  let mm = now.getMinutes().toString().padStart(2, '0');
  let ss = now.getSeconds().toString().padStart(2, '0');
  document.getElementById('liveClock').textContent = `${hh}:${mm}:${ss}`;
}
setInterval(updateClock, 1000);
updateClock();

/* *** 'H' TUŞUNA BASILINCA FORM GİZLEME *** */
document.addEventListener('keydown', function(e) {
  // 'H' veya 'h'
  if (e.key === 'h' || e.key === 'H') {
    // Checkbox'ı kapat (animasyon varsa durdur)
    document.getElementById('toggle').checked = false;

    // .container'a .hidden sınıfını toggle et
    document.querySelector('.container').classList.toggle('hidden');
  }
});
