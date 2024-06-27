[color=white]
Para criar um jogo precisamos da função "load()" que é usada para iniciar e configurar o jogo, carregando recursos como imagens, sons e fontes.

Adicionei várias imagens que usarás no jogo à pasta "mygame", com os nomes:
- nave;
- inimigos;
- mísseis;

Clique nas imagens para visualizá-las e depois escolheres para o teu jogo aquelas que mais gostaste.

Agora, o objetivo é desenhar o jogador (nave) do teu jogo.

Temos de primeiro carregar a imagem, e depois desenha-la.

1. Para carregar a imagem, escreve dentro da função "load()":
[/color] [color=blue]
   -- Variáveis da Nave do Jogador
   imagem = love.graphics.newImage("nome-imagem.png")
[/color] [color=white]

Substitui o "nome-imagen.png" pelo nome da nave que queres usar no teu jogo.

2. Depois, adiciona na função "draw()":
[/color] [color=blue]
   love.graphics.draw(imagem, 100, 100, 0, 1, 1)
[/color] [color=white]
Executa o jogo (Alt+L) e se estiver tudo correto, verás a nave desenhada no jogo.
[/color]
