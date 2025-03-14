[color=white]
Para criar um jogo precisamos da função "load()" que é 
usada para iniciar e configurar o jogo, carregando recursos 
como imagens, sons e fontes.

Adicionei várias imagens que usarás no jogo à pasta "mygame", 
com os nomes:
- background;
- bird;

Vamos começar por criar o cenário, onde o jogo vai acontecer.

1. Para carregar a imagem de fundo, escreve dentro da função "load()":
   [/color]
   [color=blue]
   -- Variável do Fundo
   background = love.graphics.newImage("background.png")
   [/color]
   [color=white]
Isto carrega a imagem "background.png", que será o fundo do nosso jogo.

2. Agora, vamos definir o tamanho da janela para se ajustar ao background:
   [/color]
   [color=blue]
   love.window.setMode(background:getWidth(), background:getHeight())
   [/color]
   [color=white]
Assim, a janela do jogo terá o mesmo tamanho da imagem de fundo.

3. Depois, adiciona na função "draw()":
   [/color]
   [color=blue]
   love.graphics.draw(background, 0, 0)
   [/color]
   [color=white]
Executa o jogo (Alt+L) e se estiver tudo correto, verás o fundo
desenhado no jogo.
[/color]
