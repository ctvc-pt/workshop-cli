[color=white]
Para criar um jogo precisamos da função "load()" que é 
usada para iniciar e configurar o jogo, carregando recursos 
como imagens, sons e fontes.

Adicionei várias imagens que usarás no jogo à pasta "mygame", 
com os nomes:
- fundo;
- passaro;

Vamos começar por criar o cenário, onde o jogo vai acontecer.

1. Para carregar a imagem de fundo, escreve dentro da função "load()":
   [/color]
   [color=blue]
   fundo = love.graphics.newImage("fundo.png")
   [/color]
   [color=white]
Isto carrega a imagem, que será o fundo do nosso jogo.

2. Agora, vamos definir o tamanho da janela para se ajustar ao fundo, 
adiciona na função "load()" o seguinte:
   [/color]
   [color=blue]
   love.window.setMode(fundo:getWidth(), fundo:getHeight())
   [/color]
   [color=white]
Assim, a janela do jogo terá o mesmo tamanho da imagem de fundo.

3. Depois, adiciona na função "draw()":
   [/color]
   [color=blue]
   love.graphics.draw(fundo, 0, 0)
   [/color]
   [color=white]
Executa o jogo (Alt+L) e se estiver tudo correto, verás o fundo
desenhado no jogo.
[/color]
