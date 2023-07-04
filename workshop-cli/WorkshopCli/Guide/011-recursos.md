
Para criar um jogo, precisamos de recursos como imagens, sons e fontes.
Vamos começar por carregar as imagens.

1. Adicionei várias imagens dentro da pasta "mygame", com os nomes de: 
 - nave;
 - inimigos;
 - mísseis;
que usarás no jogo.
Podes clicar nas imagens para as ver.


Agora vamos desenhar o jogador.

Temos de primeiro carregar a imagem, e depois desenha-la.

1. Para carregar a imagem, escreve dentro da função "load()":
   imagem = love.graphics.newImage("nome-imagem.png")

Não te esqueças de alterar "nome-imagem" pelo nome da nave que tu escolheste anteriormente.

2. Depois, escreve na função "draw()":
   love.graphics.draw(imagem, 100, 100, 0, 1, 1)

Quando vires a imagem na tela de jogo, continua para o próximo passo (alt+L).

