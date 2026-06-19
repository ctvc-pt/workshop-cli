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
Como funciona:
- O "dt" (delta time) é o tempo, em segundos, entre cada frame 
  do jogo. Multiplicar a velocidade por "dt" faz o jogo correr à 
  mesma velocidade em qualquer computador.
- O ciclo "for i=#misseis, 1, -1" percorre a lista de trás para 
  a frente. Fazemos assim para podermos remover mísseis da lista 
  sem atrapalhar o ciclo (mais à frente vais precisar de os 
  remover quando saírem do ecrã ou acertarem em inimigos).
- "atualizarMisseis(dt)" é chamada todos os frames para atualizar 
  a posição de cada míssil na lista.

Agora podes iniciar o teu jogo e divertir-te um pouco.
[/color]