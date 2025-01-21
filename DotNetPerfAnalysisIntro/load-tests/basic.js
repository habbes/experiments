import { sleep } from 'k6';
import http from 'k6/http';

const BASE_URL = __ENV.BASE_URL || "http://localhost:5208";

const places = ['Nairobi', 'Mombasa', 'Dublin', 'Redmond'];

function generateValue(size) {
  
  let s = '';

  for (let i = 0; i < size; i++) {
    const place = places[Math.floor(Math.random() * places.length)];
    const temp = (Math.random() * 50).toFixed(1);
    s += `location:${place} temp:${temp}\n`;
  }

  return s;
}

const data = generateValue(10);

export default function () {
  
  http.post(`${BASE_URL}/readings`, data, {
    headers: {
      'Content-Type': 'text/plain'
    }
  });

  sleep(1);

  http.get(`${BASE_URL}/readings/${places[Math.floor(Math.random() * places.length)]}`);

  sleep(1);
}

