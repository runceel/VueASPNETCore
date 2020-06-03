<template>
  <div>
    <div v-if="store.auth.isAuthenticated">
      認証済み：{{ store.auth.user.name }}
    </div>
    <div v-else>
      サインインしていません
    </div>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import store from '@/store';
export default Vue.extend({
  data() {
    return {
      store: store,
    }
  },
});
</script>

<style>

</style>