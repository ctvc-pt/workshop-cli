[color=white]
Vamos fazer com que os mísseis se movam e não fiquem parados quando 
disparas. 

1. Na função "load()", adiciona dentro das variáveis do míssil:
   [/color] [color=blue]
   missilVelocidade = 250
   [/color] [color=white]
2. No fim do teu código todo, adiciona esta função:
   [/color] [color=blue]
   function atualizarMisseis(dt)
        for i=#misseis, 1, -1 do
            missil = misseis[i]
            missil.posicaoX = missil.posicaoX + dt * missil.velocidade
        end
   end
   [/color] [color=white]
3. Agora dentro da função "update(dt)" adiciona por cima do ultimo 
"end" a seguinte linha:
   [/color] [color=blue]
    atualizarMisseis(dt)
   [/color] [color=white]
O código agora faz o seguinte:
 - Se a tecla "espaço" for pressionada, um míssil é disparado com 
base na velocidade do jogador. A velocidade do míssil é ajustada 
se o jogador estiver a mover-se para a esquerda ou direita. 
O míssil também é definido aqui.
 - A função "atualizarMisseis(dt)" é chamada para atualizar o 
movimento dos mísseis existentes no jogo.

Agora podes iniciar o teu jogo e divertir-te um pouco.
[/color]