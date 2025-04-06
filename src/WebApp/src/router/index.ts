import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import authorizationGuard from '@/utils/authorizationGuard'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/categories',
      name: 'categories',
      component: () => import('../views/Categories/IndexView.vue'),
    },
    {
      path: '/cart',
      name: 'cart',
      component: () => import('../views/Cart/IndexView.vue'),
      meta: {
        requireAuth: true,
      },
    },
    {
      path: '/products',
      name: 'products',
      component: () => import('../views/Products/IndexView.vue'),
    },
    {
      path: '/product-category/:slug',
      name: 'product-category',
      component: () => import('../views/ProductCategory/DetailView.vue'),
    },
    {
      path: '/products/:slug',
      name: 'products-detail',
      component: () => import('../views/Products/DetailView.vue'),
    },
    {
      path: '/user',
      children: [
        {
          path: '',
          name: 'user',
          component: () => import('../views/User/IndexView.vue'),
          meta: {
            requireAuth: true,
          },
        },
        {
          path: 'orders',
          name: 'user-orders',
          component: () => import('../views/User/Orders/IndexView.vue'),
          meta: {
            requireAuth: true,
          },
        },
        {
          path: 'orders/:id',
          name: 'orders-detail',
          component: () => import('../views/User/Orders/DetailView.vue'),
          meta: {
            requireAuth: true,
          },
        },
      ],
    },
    {
      path: '/checkout',
      name: 'checkout',
      component: () => import('../views/Checkout/IndexView.vue'),
    },
  ],
})

router.beforeResolve(authorizationGuard)

export default router
