#!/usr/bin/env bash
set -e

# ──────────────────────────────────────────────────────────────
# NutriPlan — kubectl deploy script
# Використання: ./k8s/deploy.sh [YOUR_DOCKERHUB_USERNAME]
# ──────────────────────────────────────────────────────────────

DOCKER_USER=${1:-"YOUR_DOCKERHUB_USERNAME"}

echo "==> [1/5] Підставляємо Docker Hub username: $DOCKER_USER"
find k8s/ -name "*.yaml" -exec \
  sed -i "s|YOUR_DOCKERHUB_USERNAME|$DOCKER_USER|g" {} +

echo "==> [2/5] Застосовуємо namespace"
kubectl apply -f k8s/namespace.yaml

echo "==> [3/5] Розгортаємо PostgreSQL"
kubectl apply -f k8s/postgres/secret.yaml
kubectl apply -f k8s/postgres/pvc.yaml
kubectl apply -f k8s/postgres/deployment.yaml

echo "==> [4/5] Чекаємо готовності PostgreSQL..."
kubectl rollout status deployment/postgres -n nutriplan --timeout=120s

echo "==> [5/5] Розгортаємо сервіси застосунку"
kubectl apply -f k8s/nutrition/configmap.yaml
kubectl apply -f k8s/nutrition/deployment.yaml
kubectl apply -f k8s/nutrition/service-nodeport.yaml
kubectl apply -f k8s/notification/deployment.yaml

echo ""
echo "==> Готово! Перевірка стану podів:"
kubectl get pods -n nutriplan

echo ""
echo "==> URL для доступу (minikube):"
echo "NutritionService:    $(minikube service nutrition-service-nodeport -n nutriplan --url 2>/dev/null || echo 'minikube service nutrition-service-nodeport -n nutriplan --url')"
echo "NotificationService: $(minikube service notification-service-nodeport -n nutriplan --url 2>/dev/null || echo 'minikube service notification-service-nodeport -n nutriplan --url')"
