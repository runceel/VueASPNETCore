import Vue from 'vue';
import VueRouter, { RouteConfig } from 'vue-router';
import Home from '../views/Home.vue';
import LoginCallback from '../components/api-authorization/LoginCallback.vue';
import Forecasts from '@/views/Forecasts.vue';
import { ApplicationPaths, QueryParameterNames } from '../components/api-authorization/ApiAuthorizationConstants'
import store from '@/store';
import { AuthenticationResultStatus } from '@/components/api-authorization/AuthorizeService';

const requireLoginMetadata = { requiresLogin: true };

Vue.use(VueRouter)

const routes: Array<RouteConfig> = [
  {
    path: '/',
    name: 'Home',
    component: Home,
  },
  {
    path: '/forecasts',
    name: 'Forecasts',
    component: Forecasts,
    meta: requireLoginMetadata,
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
  },
  {
    path: '/authentication/login-callback',
    name: 'LoginCallback',
    component: LoginCallback,
  }
]

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
})

router.beforeEach(async (to, from, next) => {
  await store.auth.getUser();
  if (to.matched.some(x => !!x.meta?.requiresLogin)) {
    if (store.auth.isAuthenticated) {
      next();
    } else {
      const returnUrl = to.fullPath;
      const redirectUrl = `${ApplicationPaths.Login}?${QueryParameterNames.ReturnUrl}=${encodeURI(returnUrl)}`
      const result = await store.auth.signIn({
        returnUrl: redirectUrl,
      });
      switch (result.status) {
        case AuthenticationResultStatus.Redirect:
          break;
        case AuthenticationResultStatus.Success:
          await store.auth.getUser();
          next();
          break;
        case AuthenticationResultStatus.Fail:
          next(`/error?message=${result.message}`);
          break;
        default:
          throw new Error(`Invalid status result ${result.status}.`);
      }

    }
  } else {
    next();
  }
});

export default router
