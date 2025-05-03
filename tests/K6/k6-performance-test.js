import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
  vus: 50,                // usuários virtuais
  duration: '1m',         // duração do teste
  // thresholds: {
  //   http_req_duration: ['p(95)<500'], // 95% das requisições abaixo de 500ms
  // },
};

export default function () {
  const payload = JSON.stringify({
    PedidoId: Math.floor(Math.random() * 1000000),
    ClienteId: 1,
    itens: [
      {
        ProdutoId: 1001,
        Quantidade: 2,
        Valor: 52.70
      }
    ]
  });

  const params = {
    headers: { 'Content-Type': 'application/json' },
  };

  let res = http.post('http://localhost:5000/api/pedido', payload, params);

  check(res, {
    'status é 201': (r) => r.status === 201,
  });

  sleep(0.1); // aguarda 100ms entre as requisições
}
